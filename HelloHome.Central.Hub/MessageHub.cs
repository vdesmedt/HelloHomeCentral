using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.Messages;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelloHome.Central.Hub
{
    public class MessageHub
    {
        private readonly BlockingCollection<IncomingMessage> _incomingMessages = new BlockingCollection<IncomingMessage>(new ConcurrentQueue<IncomingMessage>());
        private readonly IMessageChannel _messageChannel;

        public MessageHub(IMessageChannel messageChannel)
        {
            _messageChannel = messageChannel;            
        }

        public long LeftToProcess => _incomingMessages.Count;

        public async Task Process(CancellationToken token)
        {
            //Consumer
            var consumerTask = Task.Run(() =>
            {
                while (!_incomingMessages.IsCompleted || _incomingMessages.Count > 0)
                {
                    IncomingMessage msg;
                    if (!_incomingMessages.TryTake(out msg, 1000))
                    {
                        Console.WriteLine("No message found on queue");
                    }
                    else
                    {
                        Console.WriteLine("Processing message");
                        Thread.Sleep(5000);
                    }
                }
            });

            //Producer
            var producerTask = Task.Run(() =>            
            {
                while (!token.IsCancellationRequested)
                {
                    IncomingMessage msg = _messageChannel.TryReadNext(1000, token);
                    if (msg == null)
                    {
                        Console.WriteLine("No message found on channel after 1sec.");
                    }
                    else
                    {
                        Console.WriteLine("Adding on the queue");
                        _incomingMessages.Add(msg);
                    }
                }
                _incomingMessages.CompleteAdding();
            });
            await Task.WhenAll(consumerTask, producerTask);
        }
    }
}