﻿using System;

namespace HelloHome.Central.Common
{
    public interface ITimeProvider
    {
        DateTime UtcNow { get; }
    }

    public class TimeProvider : ITimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}