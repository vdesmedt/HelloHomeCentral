using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.Handlers;
using HelloHome.Central.Hub.Handlers.Factory;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.Messages;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using NLog;

namespace HelloHome.Central.Hub
{
    public class MessageHub
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();		

        private readonly BlockingCollection<IncomingMessage> _incomingMessages = new BlockingCollection<IncomingMessage>(new ConcurrentQueue<IncomingMessage>());
        private readonly IMessageChannel _messageChannel;
        private readonly IMessageHandlerFactory _messageHandlerFactory;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private Task _consumerTask;
        private Task _producerTask;

        public MessageHub(IMessageChannel messageChannel, IMessageHandlerFactory messageHandlerFactory)
        {
            _messageChannel = messageChannel;
            _messageHandlerFactory = messageHandlerFactory;
        }

        public long LeftToProcess => _incomingMessages.Count;

        public void Start()
        {
            var token = cts.Token;
            //Consumer
            _consumerTask = Task.Run(async () =>
            {
                while (!_incomingMessages.IsCompleted || _incomingMessages.Count > 0)
                {
                    if (!_incomingMessages.TryTake(out var msg, 1000))
                        continue;
                    Logger.Debug(() => $"Message of type {msg.GetType().ShortDisplayName()} found in queue");
                    try
                    {
                        var responses = await ProcessOne(msg, token);
                        foreach(var response in responses)
                            _messageChannel.Send(response);

                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, () => $"Exception during processing of {msg.GetType().ShortDisplayName()} : {e.Message}");
                    }
                }
            }, token);

            //Producer
            _producerTask = Task.Run(() =>            
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var msg = _messageChannel.TryReadNext();
                        if (msg == null) continue;
                        Logger.Debug(() => $"Message of type {msg.GetType().ShortDisplayName()} found in channel. Will enqueue.");
                        _incomingMessages.Add(msg, token);

                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, $"Exception from channel : {e.Message}");
                    }
                }
                _incomingMessages.CompleteAdding();
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
            var handler = _messageHandlerFactory.Create(msg);
            Logger.Debug(() => $"{handler.GetType().ShortDisplayName()} will be used to handle {msg.GetType().ShortDisplayName()}");
            var responses = await handler.HandleAsync(msg, token);
            _messageHandlerFactory.Release(handler);
            return responses;
        }

    }
}