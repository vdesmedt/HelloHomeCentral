﻿using System;

namespace HelloHome.Central.Common.Exceptions
{
    [Serializable]
    public class HelloHomeException : Exception
    {
        public string Code { get; private set; }

        public HelloHomeException(string message)
            : base(message)
        {
        }
        
        public HelloHomeException(string code, string message)
            : base(message)
        {
            Code = code;
        }
        public HelloHomeException(string code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}