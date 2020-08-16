using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Repository;
using Xunit;
using Xunit.Sdk;

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

        [Fact, Trait("skip", "ci")]
        public void EfTests()
        {
            var ctx = new DesignTimeFactoryDev().CreateDbContext(null);
            ctx.Nodes.Add(new Node
            {
                AggregatedData =  new NodeAggregatedData(),
                Metadata = new NodeMetadata(),
                RfAddress = 1,
                Ports = new List<Port>
                {
                    new PushButtonSensor
                    {
                        Name = "My first push button"
                    }
                }
            });
            ctx.SaveChanges();
        }
    }
}