using System;

namespace HelloHome.Central.Common
{
    public interface ITimeProviderFactory
    {
        ITimeProvider Create();
        void Release(ITimeProvider timeProvider);
    }

    public interface ITimeProvider
    {
        DateTime UtcNow { get; }
    }

    public class TimeProvider : ITimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}