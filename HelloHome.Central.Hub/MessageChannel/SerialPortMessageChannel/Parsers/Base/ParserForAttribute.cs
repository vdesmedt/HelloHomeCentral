using System;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ParserForAttribute : Attribute
    {
        public enum MessageType : byte
        {
            Report = 0,
            Command = 2,
        }

        public byte RawDiscrimator { get; }

        public ParserForAttribute(byte rawDiscrimator)
        {
            RawDiscrimator = rawDiscrimator;
        }
        public ParserForAttribute(MessageType type, byte discriminator)
        {
            RawDiscrimator = (byte)((byte)type + discriminator << 2);
        }
    }
}