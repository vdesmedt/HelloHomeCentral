using System;

namespace HelloHome.Central.Common.DataStructures
{
    public class CircularBuffer<T>
    where T :struct
    {
        private readonly T[] _buffer;
        private int _start;
        private int _end;

        public CircularBuffer()
            : this(capacity: 5)
        {
        }

        public CircularBuffer(int capacity)
        {
            _buffer = new T[capacity + 1];
            _start = 0;
            _end = 0;
        }
        
        public T? Write(T value)
        {
            var old = _buffer[_end];
            _buffer[_end] = value;
            _end = (_end + 1) % _buffer.Length;
            if (_end == _start)
            {
                _start = (_start + 1) % _buffer.Length;
                return old;
            }
            return null;
        }

        public T Read()
        {
            T result = _buffer[_start];
            _start = (_start + 1) % _buffer.Length;
            return result;
        }

        public int Capacity => _buffer.Length;
        public bool IsEmpty => _end == _start;
        public bool IsFull => (_end + 1) % _buffer.Length == _start;
    }
}