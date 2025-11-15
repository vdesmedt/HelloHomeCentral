using System;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Hub.IoC.Factories;
using Microsoft.Extensions.Logging;



namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialPortMessageChannel(
        ILogger<SerialPortMessageChannel> logger,
        IByteStream byteStream,
        IMessageParserFactory messageParserFactoryFactory,
        IMessageEncoderFactory messageEncoderFactoryFactory)
        : IMessageChannel
    {
        public void Open()
        {
            byteStream.Open();
        }


        public void Send(OutgoingMessage message)
        {
            var encoder = messageEncoderFactoryFactory.Build(message);
            var bytes = encoder.Encode(message);
            byteStream.Write(bytes, 0, bytes.Length);
            byteStream.Write(new byte[] {0x0D, 0x0A}, 0, 2);
            logger.LogDebug($"sent to Rfm2Pi : {message} -> {BitConverter.ToString(bytes)}-0D-0A");
        }


        private static readonly byte[] Eof = [0x0D, 0x0A];
        private const int BufSize = 100;
        private const int MaxMsgSize = 64;
        private readonly byte[] _buffer = new byte[BufSize];
        private int _currentBufferIndex = 0;
        private int _eofMatchCharCount = 0;
        private int _eofSeekIndex = 0;

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
                //Copy all bytes that can be from stream
                var byteCount = byteStream.Read(_buffer, _currentBufferIndex, BufSize - _currentBufferIndex);
                if (byteCount > 0)
                {
                    logger.LogDebug($"Found {byteCount} bytes in UART. Copied to channel buffer starting at {_currentBufferIndex}");
                    _currentBufferIndex += byteCount;
                }

                //Looking for EOF in the last byteCount of the buffer starting at previous _currentBufferIndex
                while (_eofMatchCharCount < Eof.Length && _eofSeekIndex < _currentBufferIndex)
                {
                    if (_buffer[_eofSeekIndex] == Eof[_eofMatchCharCount])
                        _eofMatchCharCount++;
                    else
                        _eofMatchCharCount = 0;
                    _eofSeekIndex++;
                }

                if (_eofMatchCharCount == Eof.Length)
                {
                    logger.LogDebug("EOF Found...  Will extract 0..EOF from channel Buffer (remaining bytes shifted left in channel buffer)");
                    _eofMatchCharCount = 0;
                    
                    //Copy buffer to msgBytes
                    var msgBytes = new byte[_eofSeekIndex];
                    for (var i = 0; i < _eofSeekIndex; i++)
                        msgBytes[i] = _buffer[i];

                    //Remove msgbytes from buffer and shift bytes left
                    for (var i = 0; i <_currentBufferIndex-_eofSeekIndex; i++)
                        _buffer[i] = _buffer[_eofSeekIndex + i];
                    _currentBufferIndex = _currentBufferIndex-_eofSeekIndex;
                    _eofSeekIndex = 0;

                    logger.LogDebug($"Found message in channel buffer : {BitConverter.ToString(msgBytes)}");
                    var parser = messageParserFactoryFactory.Build(msgBytes);
                    IncomingMessage msg = null;
                    try
                    {
                        msg = parser.Parse(msgBytes);
                    }
                    catch (ArgumentException e)
                    {
                        logger.LogError(e,"Exception thrown while parsing message.");
                        return null;
                    }

                    logger.LogInformation($"Incoming message parsed to {msg}");
                    return msg;
                }
                logger.LogDebug("EOF not found.. yet");
            }

            if (_currentBufferIndex >= MaxMsgSize) //Drop first 64 bytes and shift left in buffer
            {
                for (int i = 0; i < BufSize - MaxMsgSize; i++)
                    _buffer[i] = _buffer[MaxMsgSize + i];
                _currentBufferIndex = _eofSeekIndex = _eofMatchCharCount = 0;
            }

            return null;
        }

        public void Close()
        {
            byteStream.Close();
        }
    }
}