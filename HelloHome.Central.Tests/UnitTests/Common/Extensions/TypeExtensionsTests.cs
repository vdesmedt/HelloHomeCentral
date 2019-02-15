using System;
using HelloHome.Central.Common.Extensions;
using Xunit;

namespace HelloHome.Central.Tests.UnitTests.Common.Extensions
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void GetShortName_skips_2firsts_parts() {
            var actual = typeof(TypeExtensionsTests).GetShortName();
            var partsCount = actual.Split('.').Length;
            Assert.Equal(5, partsCount);
        }

        [Fact]
        public void GetShortName_reduces_to_capitals()
        {
            var shortened = typeof(TypeExtensionsTests).GetShortName();
            Assert.Equal("T.UT.C.E.TypeExtensionsTests", shortened);
        }
    }
}
