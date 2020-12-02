using System;
using System.Collections.Generic;

namespace HelloHome.Central.Hub.NodeBridge.Performance
{
    public class Call
    {
        private readonly DateTimeOffset startTime;
        public Call()
        {
            startTime = DateTimeOffset.Now;
        }

        public Call End()
        {
            Duration = DateTimeOffset.Now - startTime;
            return this;
        }

        public TimeSpan Duration { get; private set; }
    }
}