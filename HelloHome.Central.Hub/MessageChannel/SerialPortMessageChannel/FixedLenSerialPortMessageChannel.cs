using System;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Hub.IoC.Factories;
using Microsoft.Extensions.Logging;


namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class FixedLenSerialPortMessageChannel(
        ILogger<FixedLenSerialPortMessageChannel> logger,
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
            if (bytes.Length > 71)
                throw new ArgumentException("Message length must be lower than 71 bytes");
            byteStream.Write(new byte[] { (byte)bytes.Length }, 0, 1);
            byteStream.Write(bytes, 0, bytes.Length);
            logger.LogDebug("sent to Rfm2Pi : {message} -> {bytesLength}-{bytes}", message, bytes.Length.ToString("X2"), BitConverter.ToString(bytes));
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
                    if (byteStream.Read(_buffer, 0, 1) > 0)
                    {
                        _expectedMessageLen = _buffer[0];
                        logger.LogTrace("Expected message lenght : {expectedLenght}", _expectedMessageLen.ToString());
                        _currentBufferIndex++;
                    }
                    else
                    {
                        return null;
                    }
                }

                //Copy all bytes that can be from stream
                var byteCount = byteStream.Read(_buffer, _currentBufferIndex, _expectedMessageLen - _currentBufferIndex);
                if(byteCount == 0) 
                    return null;
                
                logger.LogDebug("Found {byte-count} bytes in UART. Copied to channel buffer starting at {currentBufferIndex}",byteCount, _currentBufferIndex);
                _currentBufferIndex += byteCount;


                if (_currentBufferIndex == _expectedMessageLen)
                {
                    logger.LogDebug("Expected size reached");
                    
                    //Copy buffer to msgBytes
                    var msgBytes = new byte[_expectedMessageLen];
                    for (var i = 0; i < _expectedMessageLen; i++)
                        msgBytes[i] = _buffer[i];

                    _currentBufferIndex = 0;
                    _expectedMessageLen = 0;

                    logger.LogDebug("Found message in channel buffer : {bytes}",BitConverter.ToString(msgBytes));
                    var parser = messageParserFactoryFactory.Build(msgBytes);
                    IncomingMessage msg = null;
                    try
                    {
                        msg = parser.Parse(msgBytes);
                    }
                    catch (ArgumentException e)
                    {
                        logger.LogError(e, "Exception thrown while parsing message.");
                        return null;
                    }

                    logger.LogInformation("Incoming message parsed to {message}", msg);
                    return msg;
                }
            }
            return null;
        }

        public void Close()
        {
            byteStream.Close();
        }
    }
}