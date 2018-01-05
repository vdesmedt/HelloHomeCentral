using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloHome.Central.Common.Exceptions;
using HelloHome.Central.Hub.Logic.RfAddressStrategy;
using Xunit;

namespace HelloHome.Central.Tests
{
    public class RfAddressStrategy
    {
        [Fact]
        public void FirstCallPass()
        {
            var sut = new FillHolesRfAddressStrategy(new List<byte>());
            var rfa = sut.FindAvailableRfAddress();
            Assert.InRange(rfa, 1, 250);
        }

        [Fact]
        public void ReturnHoleIfAny()
        {
            var existingAddresses = new List<byte> {1, 3};
            var sut = new FillHolesRfAddressStrategy(existingAddresses) {RfAddressUpperBound = 5};

            var rfa = sut.FindAvailableRfAddress();
            Assert.Equal(2, rfa);
        }

        [Fact]
        public void ReturnIfNoHole()
        {
            var existingAddresses = new List<byte> {1, 2};
            var sut = new FillHolesRfAddressStrategy(existingAddresses) {RfAddressUpperBound = 5};

            var rfa = sut.FindAvailableRfAddress();
            Assert.InRange(rfa, 3, 5);
        }

        [Fact]
        public void ThrowIfNoneAvailable()
        {
            var existingAddresses = new List<byte> {1, 2, 3};
            var sut = new FillHolesRfAddressStrategy(existingAddresses) {RfAddressUpperBound = 3};

            Assert.Throws<NoAvailableRfAddressException>(() => sut.FindAvailableRfAddress());
        }

        [Fact]
        public void FindAllAvailable()
        {
            var existingAddresses = new List<byte> {1, 2, 5, 12, 15};
            var sut = new FillHolesRfAddressStrategy(existingAddresses) {RfAddressUpperBound = 15};
            for (var i = 0; i < 10; i++)
                existingAddresses.Add(sut.FindAvailableRfAddress());
            Assert.Equal("123456789101112131415", existingAddresses.OrderBy(x => x).Aggregate(new StringBuilder(), (x, a) => x.Append(a), x => x.ToString()));
        }

        [Fact]
        public void ThreadSafe()
        {
            var existingAddresses = new List<byte> {1, 2, 5, 12, 15};
            var sut = new FillHolesRfAddressStrategy(existingAddresses) {RfAddressUpperBound = 15};
            var adr = new byte[10];
            Parallel.For(0, 10, x => adr[x] = sut.FindAvailableRfAddress());
            existingAddresses.AddRange(adr);
            Assert.Equal("123456789101112131415", existingAddresses.OrderBy(x => x).Aggregate(new StringBuilder(), (x, a) => x.Append(a), x => x.ToString()));
        }
    }
}