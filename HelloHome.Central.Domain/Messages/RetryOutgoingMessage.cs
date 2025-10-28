using System;

namespace HelloHome.Central.Domain.Messages
{
    public class RetryOutgoingMessage 
    {
        public RetryOutgoingMessage(OutgoingMessage message, int maxRetry = 3)
        {
            Message = message;
            MaxRetry = maxRetry;
        }
	    
        public OutgoingMessage Message { get; set; }
        public DateTime NextTry { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetry { get; set; }
        public bool ReadyForRetry { get; set; }
    }
}