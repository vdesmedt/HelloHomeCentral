using System;
using System.Collections.Concurrent;
using Xunit;

namespace HelloHome.Central.Tests
{
    public class Lab
    {
        [Fact]
        public void IsBlockingCollectionFIFO()
        {
            var c = new BlockingCollection<int>();
            c.Add(1);
            c.Add(2);
            var i = c.Take();
            Assert.Equal(1, i);
        }
    }
}