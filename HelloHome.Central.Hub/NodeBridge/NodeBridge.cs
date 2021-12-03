using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.IoC.Factories;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.NodeBridge.Performance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace HelloHome.Central.Hub.NodeBridge
{
    public class NodeBridge : INodeBridge
    {
        private static int instanceCount = 0;
        private int _instanceId = 0;

        private static readonly Logger Logger = LogManager.GetLogger(nameof(NodeBridge));

        private readonly BlockingCollection<IncomingMessage> _incomingMessages =
            new BlockingCollection<IncomingMessage>(new ConcurrentQueue<IncomingMessage>());

        private readonly BlockingCollection<OutgoingMessage> _outgoingMessages =
            new BlockingCollection<OutgoingMessage>(new ConcurrentQueue<OutgoingMessage>());

        private readonly IMessageChannel _messageChannel;
        private readonly IMessageHandlerFactory _messageHandlerFactory;
        private readonly ITimeProvider _timeProvider;
        private readonly IPerformanceStats _performanceStats;

        public NodeBridge(IMessageChannel messageChannel, IMessageHandlerFactory messageHandlerFactory,
            ITimeProvider timeProvider, IPerformanceStats performanceStats)
        {
            _messageChannel = messageChannel;
            _messageHandlerFactory = messageHandlerFactory;
            _timeProvider = timeProvider;
            _performanceStats = performanceStats;
            _instanceId = ++instanceCount;
            Logger.Info($"New NodBridge with instance id {_instanceId}");
        }

        public long LeftToProcess => _incomingMessages.Count;

        public void Send(OutgoingMessage message)
        {
            _outgoingMessages.Add(message);
        }

        public Task Communication(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    _messageChannel.Open();
                    Dictionary<int, int> lastMsgIdFromNodes = new Dictionary<int, int>();
                    var retryList = new Dictionary<int, RetryOutgoingMessage>();
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        //Read everything that can be
                        Logger.Trace(() => "Try read from channel");
                        var inMsg = _messageChannel.TryReadNext();
                        while (inMsg != null)
                        {
                            //Process SendingStatus
                            if (inMsg is SendingStatusReport sc)
                            {
                                Logger.Debug(() =>
                                    $"Sending report for msg {sc.MessageId} found in channel with status {(sc.Success ? "OK" : "NOK")}.");
                                if (!retryList.ContainsKey(sc.MessageId))
                                {
                                    Logger.Warn(() =>
                                        $"MessageId {sc.MessageId} not found in retry list. Ignoring");
                                }
                                else
                                {
                                    var retryMsg = retryList[sc.MessageId];
                                    if (sc.Success)
                                    {
                                        _incomingMessages.Add(new ActionConfirmedReport
                                        {
                                            Rssi = inMsg.Rssi,
                                            FromRfAddress = inMsg.FromRfAddress,
                                            ConfigmedAction = retryMsg.Message
                                        });
                                        retryList.Remove(sc.MessageId);
                                    }
                                    else if (retryMsg.RetryCount >= retryMsg.MaxRetry)
                                    {
                                        retryList.Remove(sc.MessageId);
                                        Logger.Warn(() =>
                                            $"Last try failed for message with id {sc.MessageId}. Removed from retryQueue.");
                                    }
                                    else
                                    {
                                        retryMsg.NextTry = _timeProvider.UtcNow.AddMilliseconds(1000);
                                        retryMsg.ReadyForRetry = true;
                                    }
                                }
                            }
                            //Process other incoming messages
                            else
                            {
                                if (inMsg is NodeStartedReport)
                                    lastMsgIdFromNodes[inMsg.FromRfAddress] = -1;
                                if (lastMsgIdFromNodes.ContainsKey(inMsg.FromRfAddress) &&
                                    lastMsgIdFromNodes[inMsg.FromRfAddress] == inMsg.MsgId)
                                {
                                    Logger.Info(() =>
                                        $"Message with Id {inMsg.MsgId} coming from RFAddr {inMsg.FromRfAddress} was already added in queue and will be dismissed.");
                                }
                                else
                                {
                                    Logger.Debug(() =>
                                        $"Message of type {inMsg.GetType().Name} found in channel. Will enqueue.");
                                    _incomingMessages.Add(inMsg, cancellationToken);
                                    lastMsgIdFromNodes[inMsg.FromRfAddress] = inMsg.MsgId;
                                }
                            }

                            inMsg = _messageChannel.TryReadNext();
                        }

                        //Write any left message from outgoingQueue
                        Logger.Trace(() => "Try read from Queue");
                        while (!_outgoingMessages.IsCompleted && _outgoingMessages.Count > 0)
                        {
                            if (_outgoingMessages.TryTake(out var outMsg, 10))
                            {
                                Logger.Debug(() =>
                                    $"Message of type {outMsg.GetType().Name} with id {outMsg.MessageId} found in queue. Will send.");
                                _messageChannel.Send(outMsg);
                                retryList.Add(outMsg.MessageId, new RetryOutgoingMessage(outMsg));
                            }
                        }

                        //Retry
                        Logger.Trace(() => "Retry sendings");
                        var pivot = _timeProvider.UtcNow;
                        foreach (var retryMsg in retryList.Values)
                        {
                            if (retryMsg.ReadyForRetry && retryMsg.NextTry < pivot)
                            {
                                Logger.Debug(() =>
                                    $"Will retry messageId {retryMsg.Message.MessageId} ({retryMsg.Message.GetType().Name})");
                                _messageChannel.Send(retryMsg.Message);
                                retryMsg.ReadyForRetry = false;
                                retryMsg.RetryCount++;
                            }
                        }
                    }

                    _incomingMessages.CompleteAdding();
                    _messageChannel.Close();
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Exception in Communication Task : {e.Message}");
                }
            }, cancellationToken);
        }

        public Task Processing(CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested &&
                       (!_incomingMessages.IsCompleted || _incomingMessages.Count > 0))
                {
                    if (!_incomingMessages.TryTake(out var inMsg, 1000))
                        continue;
                    var call = _performanceStats.StartCall();
                    Logger.Debug(() => $"Message of type {inMsg.GetType().Name} found in queue");
                    try
                    {
                        var responses = await ProcessOne(inMsg, cancellationToken);
                        foreach (var response in responses)
                        {
                            Logger.Debug(() => $"Enqueuing response {response}");
                            _outgoingMessages.Add(response, cancellationToken);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, () => $"Exception during processing of {inMsg.GetType().Name} : {e.Message}");
                    }
                    finally
                    {
                        _performanceStats.AddHandlerCall(call.End());
                    }
                }
            });
        }

        public async Task<IList<OutgoingMessage>> ProcessOne(IncomingMessage msg, CancellationToken token)
        {
            var ts = Stopwatch.StartNew();
            using var scopedHandler = _messageHandlerFactory.BuildInNestedScope(msg);
            Logger.Debug(() => $"{scopedHandler.Handler.GetType().Name} will be used to handle {msg.GetType().Name}");
            var responses = await scopedHandler.Handler.HandleAsync(msg, token);
            ts.Stop();
            Logger.Debug(() =>
                $"{scopedHandler.Handler.GetType().Name} has finnish handling {msg.GetType().Name} in {ts.ElapsedMilliseconds} ms.");

            return responses;
        }
    }
}