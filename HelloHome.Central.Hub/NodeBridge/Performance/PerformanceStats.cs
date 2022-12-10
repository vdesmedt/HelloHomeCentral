using System;
using System.Collections.Generic;
using HelloHome.Central.Common.DataStructures;

namespace HelloHome.Central.Hub.NodeBridge.Performance
{
    public interface IPerformanceStats
    {
        Call StartCall();
        void AddHandlerCall(Call call);
        long CallCount { get; }
    }

    public class PerformanceStats : IPerformanceStats
    {
        private long _handlerCallCount = 0;
        private readonly CircularBuffer<long> _lasHandlerDurations = new CircularBuffer<long>(10);
        private float _totalHandlerDuration = 0;
        
        public Call StartCall()
        {
            return new Call();
        }

        public void AddHandlerCall(Call call)
        {
            _handlerCallCount++;
            _totalHandlerDuration += call.Duration.Ticks;
            var rejected = _lasHandlerDurations.Write(call.Duration.Ticks);
            _totalHandlerDuration -= rejected ?? 0;
        }

        public long CallCount => _handlerCallCount;
        public float AverageHandlerDuration => (_totalHandlerDuration /TimeSpan.TicksPerMillisecond) / (_handlerCallCount > 10 ? 10 : _handlerCallCount);
    }
}