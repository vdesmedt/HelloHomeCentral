using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel
{
    public class RandomMessageChannel : IMessageChannel
    {
        private readonly Random _rnd = new Random();

        public IncomingMessage TryReadNext()
        {
            var d = DateTime.Now.AddMilliseconds(1000);
            while (DateTime.Now < d)
            {
                var next = _rnd.Next(10);
                switch (next)
                {
                    case 1:
                        return new NodeStartedReport
                        {
                            FromRfAddress = 2,
                            Version = "aaabbbc", 
                            Signature = 2
                        };
                }

                Thread.Sleep(100);
            }

            return null;
        }

        public void Send(OutgoingMessage message)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            
        }
    }
}