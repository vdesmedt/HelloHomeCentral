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

        public byte RawDiscriminator { get; }

        public ParserForAttribute(byte rawDiscriminator)
        {
            RawDiscriminator = rawDiscriminator;
        }
        public ParserForAttribute(MessageType type, byte discriminator)
        {
            RawDiscriminator = (byte)((byte)type + discriminator << 2);
        }
    }
}