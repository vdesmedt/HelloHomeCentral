using System;

namespace HelloHome.Central.Common.Extensions
{
	public static class DateTimeExtensions
	{
		static readonly DateTime epochRef = new DateTime (1970, 1, 1);

		public static long ToEpoch (this DateTime dt)
		{
			return (long)((epochRef - dt).TotalSeconds);
		}
		public static DateTime Round (this DateTime date, TimeSpan span)
		{
			long ticks = (date.Ticks + (span.Ticks / 2) + 1) / span.Ticks;
			return new DateTime (ticks * span.Ticks);
		}
		public static DateTime Floor (this DateTime date, TimeSpan span)
		{
			long ticks = (date.Ticks / span.Ticks);
			return new DateTime (ticks * span.Ticks);
		}
		public static DateTime Ceil (this DateTime date, TimeSpan span)
		{
			long ticks = (date.Ticks + span.Ticks - 1) / span.Ticks;
			return new DateTime (ticks * span.Ticks);
		}
	}
}

