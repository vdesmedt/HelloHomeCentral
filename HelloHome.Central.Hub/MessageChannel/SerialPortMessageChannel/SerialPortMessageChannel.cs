using System;
using System.Net;
using System.Threading;
using HelloHome.Central.Hub.IoC.Factories;
using HelloHome.Central.Hub.MessageChannel.Messages;
using NLog;


namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialPortMessageChannel : IMessageChannel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IByteStream _byteStream;
        private readonly IMessageParserFactory _messageParserFactory;
        private readonly IMessageEncoderFactory _messageEncoderFactory;

        public SerialPortMessageChannel(IByteStream byteStream, IMessageParserFactory messageParserFactoryFactory,
            IMessageEncoderFactory messageEncoderFactoryFactory)
        {
            _byteStream = byteStream;
            _messageParserFactory = messageParserFactoryFactory;
            _messageEncoderFactory = messageEncoderFactoryFactory;
        }

        public void Open()
        {
            _byteStream.Open();
        }


        public void Send(OutgoingMessage message)
        {
            var encoder = _messageEncoderFactory.Build(message);
            var bytes = encoder.Encode(message);
            _byteStream.Write(new[] {message.ToRfAddress}, 0, 1);
            _byteStream.Write(bytes, 0, bytes.Length);
            _byteStream.Write(new byte[] {0x0D, 0x0A}, 0, 2);
            Logger.Debug(() =>
                $"sent to Rfm2Pi : {message} -> {BitConverter.ToString(new byte[] {message.ToRfAddress})}-{BitConverter.ToString(bytes)}-0D-0A");
        }


        private const int BufferSize = 100;
        readonly byte[] _buffer = new byte[BufferSize];
        int _currentBufferIndex = 0;

        /// <summary>
        /// Return a message or null if timeout passes without a complete message ending with Eof can be found
        /// </summary>
        /// <returns></returns>
        public IncomingMessage TryReadNext()
        {
            if (_byteStream.ByteAvailable() == 0)
                return null;
            
            // at start, some left overs might still be at the beginning of the buffer,
            // currentIndex can therefore be greater than 0
            byte[] eof = {0x0D, 0x0A};

            var eofMatchCharCount = 0;
            while (eofMatchCharCount < eof.Length)
            {
                if (_byteStream.ByteAvailable() == 0)
                {
                    Thread.Sleep(50);
                    continue;
                }
                var byteCount = _byteStream.Read(_buffer, _currentBufferIndex, BufferSize - _currentBufferIndex);
                Logger.Debug($"Found {byteCount} bytes in UART");
                _currentBufferIndex += byteCount;
                //Looking for EOF
                while (eofMatchCharCount < eof.Length && byteCount > 0)
                {
                    if (_buffer[_currentBufferIndex - byteCount] == eof[eofMatchCharCount])
                        eofMatchCharCount++;
                    else
                        eofMatchCharCount = 0;
                    byteCount--;
                }

                if (eofMatchCharCount != eof.Length)
                {
                    Logger.Debug("EOF not found.. yet");
                    return null;
                }
                Logger.Debug("EOF Found...  Will extract from start to EOF from Buffer (remaining bytes shifted left in buffer)");
                
                //Copy buffer to msgBytes
                var msgBytes = new byte[_currentBufferIndex - byteCount];
                for (var i = 0; i < _currentBufferIndex - byteCount; i++)
                    msgBytes[i] = _buffer[i];

                //Remove msgbytes from buffer and shift bytes left
                for (var i = 0; i < byteCount; i++)
                    _buffer[i] = _buffer[_currentBufferIndex - byteCount + i];
                _currentBufferIndex = byteCount;

                Logger.Debug(() => $"Found message in buffer : {BitConverter.ToString(msgBytes)}");
                var parser = _messageParserFactory.Build(msgBytes);
                var msg = parser.Parse(msgBytes);
                Logger.Info(() => $"Incoming message parsed to {msg}");
                return msg;
            }

            return null;
        }

        public void Close()
        {
            _byteStream.Close();
        }
    }
}