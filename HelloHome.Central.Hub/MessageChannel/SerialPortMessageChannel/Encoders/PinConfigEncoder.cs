using System;
using System.Collections;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
	public class PinConfigEncoder
	{
		public byte EncodePins(byte pin1, byte pin2)
		{
		    return (byte)((EncodePins(pin1) << 4) + EncodePins(pin2));
		}

		public byte EncodePins(byte pin)
		{
		    if (pin == 0) return 0;
		    if(pin >= _map.Length || _map[pin] == 0)
		        throw new Exception($"Pin {pin} is unavailable.");
		    return _map[pin];
		}

	    //Only D4-D7, D14-D17 and A6-A7 are availble. That is 10 Pins in total
	    private readonly byte[] _map = {0, 0, 0, 0, 1, 2, 3, 4, 0, 0, 0, 0, 0, 0, 5, 6, 7, 8, 0, 0, 9, 10};				
	}
}

