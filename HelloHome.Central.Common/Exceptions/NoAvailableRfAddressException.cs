using System;

namespace HelloHome.Central.Common.Exceptions
{
    public class NoAvailableRfAddressException : HelloHomeException
    {    
        public NoAvailableRfAddressException(bool tryAgain) : base("NO_RFADDR_AVAIL",
            tryAgain
                ? "Could not find an available RfAddress in range right now. It might be the result of too much parrallel request. Try again later."
                : "No available rf address left in range")
        {
        }
    }
}