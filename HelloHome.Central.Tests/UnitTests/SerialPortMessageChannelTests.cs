using System;
using System.Collections.Generic;
using System.Linq;
using HelloHome.Central.Hub.IoC.Factories;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;
using Microsoft.VisualBasic;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace HelloHome.Central.Tests.UnitTests
{
    public class DummyInMessage : IncomingMessage
    {
        public byte[] Bytes { get; set; }
    }

    public class DummyParser : IMessageParser
    {
        public IncomingMessage Parse(byte[] record)
        {
            return new DummyInMessage {Bytes = record};
        }
    }

    public class SerialPortMessageChannelTests
    {
        private readonly SerialPortMessageChannel _sut;
        private readonly Mock<IByteStream> _byteStreamMock;
        private readonly Queue<byte[]> _byteSequenceQueue = new Queue<byte[]>();


        private readonly Mock<IMessageParserFactory> _messageParserFactory;
        private readonly Mock<IMessageEncoderFactory> _messageEncoderFactory;


        public SerialPortMessageChannelTests()
        {
            _byteStreamMock = new Mock<IByteStream>();
            _byteStreamMock.Setup(_ => _.ByteAvailable()).Returns(() =>
            {
                if (_byteSequenceQueue.Count == 0)
                    return 0;
                return _byteSequenceQueue.Peek().Length;
            });
            _byteStreamMock.Setup(_ => _.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns((byte[] ba, int o, int c) =>
                {
                    if (_byteSequenceQueue.Count == 0)
                        return 0;
                    var bl = _byteSequenceQueue.Dequeue();
                    var byteToCopy = Math.Min(bl.Length, c);
                    for (var i = 0; i < byteToCopy; i++)
                        ba[o + i] = bl[i];
                    return byteToCopy;
                });

            _messageParserFactory = new Mock<IMessageParserFactory>();
            _messageParserFactory.Setup(_ => _.Build(It.IsAny<byte[]>())).Returns((byte[] ba) => new DummyParser());

            _messageEncoderFactory = new Mock<IMessageEncoderFactory>();

            _sut = new SerialPortMessageChannel(_byteStreamMock.Object, _messageParserFactory.Object,
                _messageEncoderFactory.Object);
        }

        [Fact]
        public void ReadUntil0D0A()
        {
            _byteSequenceQueue.Enqueue(new byte[] {0x0, 0x1, 0x0d, 0x0a});

            var msg = _sut.TryReadNext() as DummyInMessage;
            Assert.NotNull(msg);
            Assert.Equal(4, msg.Bytes.Length);
            Assert.Equal(new byte[] {0, 1, 13, 10}, msg.Bytes);
        }

        [Fact]
        public void TryNext_WaitsFor_EOFOrMaxBytes()
        {
            _byteSequenceQueue.Enqueue(Enumerable.Range(0, 100).Select(_ => (byte) _).ToArray());
            var msg = _sut.TryReadNext();
            Assert.Null(msg);
        }

        [Fact]
        public void TryNext_ReturnsNull_IfNoEof()
        {
        }

        [Fact]
        //That says that if 64 byte limit is reached,
        //the next Eof will be seeked before starting to interpret again
        public void TryNext_InterpretsFromEofToNextEofOnly()
        {
        }

        [Fact]
        public void ReadUntil0D0AEvenAcrossTwoByteSeries()
        {
            _byteSequenceQueue.Enqueue(new byte[] {0x0, 0x1, 0x0d});
            _byteSequenceQueue.Enqueue(new byte[] {0x0a, 0x2, 0x3, 0x04, 0x0d, 0x0a});

            var msg = _sut.TryReadNext() as DummyInMessage;
            Assert.NotNull(msg);
            Assert.Equal(4, msg.Bytes.Length);
            Assert.Equal(new byte[] {0, 1, 13, 10}, msg.Bytes);
        }

        [Fact]
        public void ReadNext_WillSee_TwoMessages()
        {
            _byteSequenceQueue.Enqueue(new byte[] {0x0, 0x1, 0x0d});
            _byteSequenceQueue.Enqueue(new byte[] {0x0a, 0x2, 0x3, 0x04, 0x0d, 0x0a});
            var msg1 = _sut.TryReadNext() as DummyInMessage;
            var msg2 = _sut.TryReadNext() as DummyInMessage;
            
            Assert.NotNull(msg1);
            Assert.Equal(new byte[] {0,1,13,10}, msg1.Bytes);
            Assert.NotNull(msg2);
            Assert.Equal(new byte[] {2,3,4,13,10}, msg2.Bytes);
        }
    }
}