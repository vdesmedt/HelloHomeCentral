using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
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
                        Logger.Debug(() => $"Processing message of type {msg.GetType().ShortDisplayName()}");
                        var handler = _messageHandlerFactory.Create(msg);
                        var responses = await handler.HandleAsync(msg, token);
                        foreach(var response in responses)
                            _messageChannel.Send(response);
                        _messageHandlerFactory.Release(handler);
                    }
                }
            });

            //Producer
            var producerTask = Task.Run(() =>            
            {
                while (!token.IsCancellationRequested)
                {
                    var msg = _messageChannel.TryReadNext(1000, token);
                    if (msg == null) continue;
                    Logger.Debug(() => $"Message of type {msg.GetType().ShortDisplayName()} found in channel. Will enqueue.");
                    _incomingMessages.Add(msg);
                }
                _incomingMessages.CompleteAdding();
            });
            
            await Task.WhenAll(consumerTask, producerTask);
        }
    }
}