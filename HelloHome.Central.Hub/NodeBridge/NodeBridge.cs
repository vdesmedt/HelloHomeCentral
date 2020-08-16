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

        public NodeBridge(IMessageChannel messageChannel, IMessageHandlerFactory messageHandlerFactory,
            ITimeProvider timeProvider)
        {
            _messageChannel = messageChannel;
            _messageHandlerFactory = messageHandlerFactory;
            _timeProvider = timeProvider;
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
                                    Logger.Warn($"MessageId {sc.MessageId} not found in retry list. Ignoring");
                                }
                                else
                                {
                                    if (sc.Success)
                                    {
                                        _incomingMessages.Add(new ActionConfirmedReport
                                        {
                                            Rssi = inMsg.Rssi,
                                            FromRfAddress = inMsg.FromRfAddress,
                                            ConfigmedAction =retryList[sc.MessageId].Message 
                                        });
                                        retryList.Remove(sc.MessageId);
                                    }
                                    else if (retryList[sc.MessageId].RetryCount >= retryList[sc.MessageId].MaxRetry)
                                    {
                                        retryList.Remove(sc.MessageId);
                                        Logger.Warn(() =>
                                            $"Last try failed for message with id {sc.MessageId}. Removed from retryQueue.");
                                    }
                                    else
                                    {
                                        retryList[sc.MessageId].NextTry = _timeProvider.UtcNow.AddMilliseconds(1000);
                                        retryList[sc.MessageId].ReadyForRetry = true;
                                    }
                                }
                            }
                            //Process other incoming messages
                            else
                            {
                                Logger.Debug(() =>
                                    $"Message of type {inMsg.GetType().Name} found in channel. Will enqueue.");
                                _incomingMessages.Add(inMsg, cancellationToken);
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