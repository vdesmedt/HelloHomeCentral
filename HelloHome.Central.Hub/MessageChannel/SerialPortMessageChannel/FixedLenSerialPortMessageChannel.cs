using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using HelloHome.Central.Hub.IoC.Factories;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using NLog;


namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class FixedLenSerialPortMessageChannel : IMessageChannel
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(FixedLenSerialPortMessageChannel));

        private readonly IByteStream _byteStream;
        private readonly IMessageParserFactory _messageParserFactory;
        private readonly IMessageEncoderFactory _messageEncoderFactory;

        public FixedLenSerialPortMessageChannel(IByteStream byteStream, IMessageParserFactory messageParserFactoryFactory,
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
            if (bytes.Length > 71)
                throw new ArgumentException("Message length must be lower than 71 bytes");
            _byteStream.Write(new byte[] { (byte)bytes.Length }, 0, 1);
            _byteStream.Write(bytes, 0, bytes.Length);
            Logger.Debug(() =>
                $"sent to Rfm2Pi : {message} -> {bytes.Length.ToString("X2")}-{BitConverter.ToString(bytes)}");
        }


        private const int BufSize = 100;
        private const int MaxMsgSize = 64;
        private readonly byte[] _buffer = new byte[BufSize];
        private int _currentBufferIndex = 0;
        private byte _expectedMessageLen = 0;
        /// <summary>
        /// Return a message or null if timeout passes without a complete message ending with Eof can be found
        /// </summary>
        /// <returns></returns>
        public IncomingMessage TryReadNext()
        {
            // at start, some left overs might still be at the beginning of the buffer,
            // currentIndex can therefore be greater than 0
            while (_currentBufferIndex < MaxMsgSize)
            {
                if(_currentBufferIndex == 0)
                {
                    if (_byteStream.Read(_buffer, 0, 1) > 0)
                    {
                        _expectedMessageLen = _buffer[0];
                        Logger.Trace(() => $"Expected message lenght : {_expectedMessageLen.ToString()}");
                        _currentBufferIndex++;
                    }
                    else
                    {
                        return null;
                    }
                }

                //Copy all bytes that can be from stream
                var byteCount = _byteStream.Read(_buffer, _currentBufferIndex, _expectedMessageLen - _currentBufferIndex);
                if(byteCount == 0) 
                    return null;
                
                Logger.Debug($"Found {byteCount.ToString()} bytes in UART. Copied to channel buffer starting at {_currentBufferIndex.ToString()}");
                _currentBufferIndex += byteCount;


                if (_currentBufferIndex == _expectedMessageLen)
                {
                    Logger.Debug("Expected size reached");
                    
                    //Copy buffer to msgBytes
                    var msgBytes = new byte[_expectedMessageLen];
                    for (var i = 0; i < _expectedMessageLen; i++)
                        msgBytes[i] = _buffer[i];

                    _currentBufferIndex = 0;
                    _expectedMessageLen = 0;

                    Logger.Debug(() => $"Found message in channel buffer : {BitConverter.ToString(msgBytes)}");
                    var parser = _messageParserFactory.Build(msgBytes);
                    IncomingMessage msg = null;
                    try
                    {
                        msg = parser.Parse(msgBytes);
                    }
                    catch (ArgumentException e)
                    {
                        Logger.Error(e);
                        return null;
                    }

                    Logger.Info(() => $"Incoming message parsed to {msg}");
                    return msg;
                }
            }
            return null;
        }

        public void Close()
        {
            _byteStream.Close();
        }
    }
}