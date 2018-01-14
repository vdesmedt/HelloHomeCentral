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

        public MessageHub(IMessageChannel messageChannel, IMessageHandlerFactory messageHandlerFactory)
        {
            _messageChannel = messageChannel;
            _messageHandlerFactory = messageHandlerFactory;
        }

        public long LeftToProcess => _incomingMessages.Count;

        public async Task Process(CancellationToken token)
        {
            //Consumer
            var consumerTask = Task.Run(async () =>
            {
                while (!_incomingMessages.IsCompleted || _incomingMessages.Count > 0)
                {
                    IncomingMessage msg;
                    if (_incomingMessages.TryTake(out msg, 1000))
                    {
                        Logger.Debug(() => $"Message of type {msg.GetType().ShortDisplayName()} found in queue");
                        try
                        {
                            var responses = await ProcessOne(msg, token);
                            foreach(var response in responses)
                                await _messageChannel.SendAsync(response, CancellationToken.None);

                        }
                        catch (Exception e)
                        {
                            Logger.Error(e, () => $"Exception during processing of {msg.GetType().ShortDisplayName()} : {e.Message}");
                        }
                    }
                }
            });

            //Producer
            var producerTask = Task.Run(async () =>            
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var msg = await _messageChannel.TryReadNextAsync(token);
                        if (msg == null) continue;
                        Logger.Debug(() => $"Message of type {msg.GetType().ShortDisplayName()} found in channel. Will enqueue.");
                        _incomingMessages.Add(msg);

                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, $"Exception from channel : {e.Message}");
                    }
                }
                _incomingMessages.CompleteAdding();
            });
            
            await Task.WhenAll(consumerTask, producerTask);
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