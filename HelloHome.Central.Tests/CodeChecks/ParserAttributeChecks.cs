using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;
using JasperFx.Core.Reflection;
using Xunit;

namespace HelloHome.Central.Tests.CodeChecks
{
    public class ParserAttributeChecks
    {
        [Fact]
        public void AllParsersHaveParserForAttribute()
        {
            var parsers = typeof(IMessageParser).Assembly.GetTypes().Where(t =>  t.IsConcrete() && typeof(IMessageParser).IsAssignableFrom(t));
            Assert.All(parsers, p => Assert.True(p.HasAttribute<ParserForAttribute>()));
        }

        [Fact]
        public void ParserForValuesAreUnique()
        {
            var parsers = typeof(IMessageParser).Assembly.GetTypes().Where(t => t.IsConcrete() && typeof(IMessageParser).IsAssignableFrom(t));


            Assert.All(parsers, p =>
                Assert.True(p.GetCustomAttribute<ParserForAttribute>(true) is NonDiscriminatedParserAttribute
                            || parsers.Count(q =>
                                q.GetCustomAttribute<ParserForAttribute>().RawDiscriminator ==
                                p.GetCustomAttribute<ParserForAttribute>().RawDiscriminator) == 1));
        }
    }
}