namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    public interface IMessageParserFactory
    {
        IMessageParser Create(byte[] bytes);
        void Release(IMessageParser parser);
    }
}