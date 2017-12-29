using System;
using System.Collections;
using System.Threading;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel
{
    public class RandomMessageChannel : IMessageChannel
    {
        private Random _rnd = new Random();

        public IncomingMessage TryReadNext(int millisecond, CancellationToken cancellationToken)
        {
            var d = DateTime.Now.AddMilliseconds(millisecond);
            while (!cancellationToken.IsCancellationRequested && DateTime.Now < d)
            {
                var next = _rnd.Next(10);
                switch (next)
                {
                    case 1:
                        return new NodeStartedReport
                        {
                            FromRfAddress = 2,
                            Major = 1,
                            Minor = 1,
                            Signature = 2
                        };
                }

                Thread.Sleep(100);
            }

            return null;
        }

        public void Send(OutgoingMessage message)
        {
        }
    }
}