using System;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers;
using Microsoft.Extensions.Configuration;
using NLog;
using RJCP.IO.Ports;

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
            var encoder = _messageEncoderFactory.Create(message);
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
            // at start, some left overs might still be at the begining of the buffer,
            // currentIndex can therefore be greater than 0
            byte[] eof = {0x0D, 0x0A};

            var eofMatchCharCount = 0;
            while (eofMatchCharCount < eof.Length)
            {
                var byteCount = _byteStream.Read(_buffer, _currentBufferIndex, BufferSize - _currentBufferIndex);

                _currentBufferIndex += byteCount;
                while (eofMatchCharCount < eof.Length && byteCount > 0)
                {
                    if (_buffer[_currentBufferIndex - byteCount] == eof[eofMatchCharCount])
                        eofMatchCharCount++;
                    else
                        eofMatchCharCount = 0;
                    byteCount--;
                }

                if (eofMatchCharCount != eof.Length)
                    return null;

                //Copy buffer to msgBytes
                var msgBytes = new byte[_currentBufferIndex - byteCount];
                for (var i = 0; i < _currentBufferIndex - byteCount; i++)
                    msgBytes[i] = _buffer[i];

                //Remove msgbytes from buffer and shift bytes left
                for (var i = 0; i < byteCount; i++)
                    _buffer[i] = _buffer[_currentBufferIndex - byteCount + i];
                _currentBufferIndex = byteCount;

                Logger.Debug(() => $"Found message in buffer : {BitConverter.ToString(msgBytes)}");
                var parser = _messageParserFactory.Create(msgBytes);
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