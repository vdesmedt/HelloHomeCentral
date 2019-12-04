using Xunit;

namespace HelloHome.Central.Tests.UnitTests
{
    public class Test
    {
        private int i = 0;
        private static int j = 1;
        public Test()
        {
            i = 1;
        }

        [Fact]
        public void ConstructorRunForEveryTest_Part1()
        {
            i++;
            Assert.Equal(2, i);
        }

        [Fact]
        public void ConstructorRunForEveryTest_Part2()
        {
            i++;
            Assert.Equal(2, i);
        }

        [Fact]
        public void StaticRemains_Part1()
        {
            j++;
            Assert.Equal(2, j);
        }

        [Fact]
        public void StaticRemains_Part2()
        {
            j++;
            Assert.Equal(3, j);
        }
    }
}