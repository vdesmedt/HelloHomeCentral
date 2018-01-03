using System;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ParserForAttribute : Attribute
    {
        public byte DiscrimatorByte { get; }

        public ParserForAttribute(byte discrimatorByte)
        {
            DiscrimatorByte = discrimatorByte;
        }
    }
}