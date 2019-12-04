using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.IoC.Factories;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using NLog;

namespace HelloHome.Central.Hub
{
    public class MessageHub
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(MessageHub));

        private readonly BlockingCollection<IncomingMessage> _incomingMessages = new BlockingCollection<IncomingMessage>(new ConcurrentQueue<IncomingMessage>());
        private readonly BlockingCollection<OutgoingMessage> _outgoingMessages = new BlockingCollection<OutgoingMessage>(new ConcurrentQueue<OutgoingMessage>());
        private readonly IMessageChannel _messageChannel;
        private readonly IMessageHandlerFactory _messageHandlerFactory;
        private readonly ITimeProvider _timeProvider;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private Task _consumerTask;
        private Task _producerTask;

        public MessageHub(IMessageChannel messageChannel, IMessageHandlerFactory messageHandlerFactory, ITimeProvider timeProvider)
        {
            _messageChannel = messageChannel;
            _messageHandlerFactory = messageHandlerFactory;
            _timeProvider = timeProvider;
        }

        public long LeftToProcess => _incomingMessages.Count;

        public void Start()
        {
            _messageChannel.Open();
            var token = cts.Token;
            
            //Communicate through channel
            _producerTask = Task.Run(() =>
            {
                var retryList = new Dictionary<int, RetryOutgoingMessage>();
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        //Read everything that can be
                        var inMsg = _messageChannel.TryReadNext();
                        while (inMsg != null) 
                        {
                            //Process SendingStatus
                            if (inMsg is SendingStatusReport sc)
                            {
                                Logger.Debug(() => $"Sending report for msg {sc.MessageId} found in channel with status {(sc.Success?"OK":"NOK")}.");
                                if (sc.Success)
                                    retryList.Remove(sc.MessageId);
                                else if(retryList[sc.MessageId].RetryCount >= retryList[sc.MessageId].MaxRetry)
                                {
                                    retryList.Remove(sc.MessageId);
                                    Logger.Warn(() => $"Last try failed for message with id {sc.MessageId}. Removed from retryQueue.");
                                }
                                else
                                {
                                    retryList[sc.MessageId].NextTry = _timeProvider.UtcNow.AddMilliseconds(1000);
                                    retryList[sc.MessageId].ReadyForRetry = true;
                                }
                            }
                            //Process other incoming messages
                            else
                            {
                                Logger.Debug(() => $"Message of type {inMsg.GetType().Name} found in channel. Will enqueue.");
                                _incomingMessages.Add(inMsg, token);
                            } 

                            inMsg = _messageChannel.TryReadNext();
                        }
                        
                        //Write any left message from outgoingQueue
                        while (!_outgoingMessages.IsCompleted && _outgoingMessages.Count > 0)
                        {
                            if(_outgoingMessages.TryTake(out var outMsg, 100))
                            {
                                Logger.Debug(() => $"Message of type {outMsg.GetType().Name} with id {outMsg.MessageId} found in queue. Will send.");
                                _messageChannel.Send(outMsg);
                                retryList.Add(outMsg.MessageId, new RetryOutgoingMessage(outMsg));
                            }
                        }
                        
                        //Retry
                        var pivot = _timeProvider.UtcNow;
                        foreach (var retryMsg in retryList.Values)
                        {
                            if (retryMsg.ReadyForRetry && retryMsg.NextTry < pivot)
                            {
                                Logger.Debug(() => $"Will retry messageId {retryMsg.Message.MessageId} ({retryMsg.Message.GetType().Name})");
                                _messageChannel.Send(retryMsg.Message);
                                retryMsg.ReadyForRetry = false;
                                retryMsg.RetryCount++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, $"Exception from channel : {e.Message}");
                    }
                }
                _incomingMessages.CompleteAdding();
            }, token);    
            
            //Processing
            _consumerTask = Task.Run(async () =>
            {
                while (!_incomingMessages.IsCompleted || _incomingMessages.Count > 0)
                {
                    if (!_incomingMessages.TryTake(out var inMsg, 1000)) 
                        continue;
                    Logger.Debug(() => $"Message of type {inMsg.GetType().Name} found in queue");
                    try
                    {
                        var responses = await ProcessOne(inMsg, token);
                        foreach (var response in responses)
                            _outgoingMessages.Add(response, token);

                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, () => $"Exception during processing of {inMsg.GetType().Name} : {e.Message}");
                    }
                }
            }, token);
        }

        public void Stop()
        {
            cts.Cancel();
            _messageChannel.Close();
            var l = LeftToProcess;
            while (l > 0)
            {
                Console.WriteLine($"{LeftToProcess} message(s) left to process...");
                while (l == LeftToProcess) ;
                Thread.Sleep(100);
                l = LeftToProcess;
            }
        }

        public async Task<IList<OutgoingMessage>> ProcessOne(IncomingMessage msg, CancellationToken token)
        {
            var ts = Stopwatch.StartNew();
            using var scopedHandler = _messageHandlerFactory.BuildInNestedScope(msg);
            Logger.Debug(() => $"{scopedHandler.Handler.GetType().Name} will be used to handle {msg.GetType().Name}");
            var responses = await scopedHandler.Handler.HandleAsync(msg, token);    
            ts.Stop();
            Logger.Debug(() => $"{scopedHandler.Handler.GetType().Name} has finnish handling {msg.GetType().Name} in {ts.ElapsedMilliseconds} ms.");
            
            return responses;
        }

    }
}