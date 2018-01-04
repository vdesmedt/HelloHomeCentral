using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace HelloHome.Central.Hub.Logic.RfAddressStrategy
{
	public class FindHoleRfAddressStrategy : IRfAddressStrategy
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();		

		private readonly SortedSet<byte> _exisitingRfAddresses;
		private readonly Random _rnd;
		
		public int RfADdressUpperBound { get; set; }

		public FindHoleRfAddressStrategy (IEnumerable<byte> exisitingRfAddresses)
		{
			_exisitingRfAddresses = new SortedSet<byte>(exisitingRfAddresses);
			_rnd = new Random ();
		}


		#region IRfNodeIdGeneratorStrategy implementation

		private byte FindRfAddressInternal ()
		{
			if (!_exisitingRfAddresses.Any ())
				return 1;
			var maxExisting = _exisitingRfAddresses.Max ();
			var holes = Enumerable.Range(1, maxExisting).Select(i => (byte)i).Where (i => !_exisitingRfAddresses.Contains(i)).ToList ();
			if (holes.Any()) 
				return holes [_rnd.Next(holes.Count - 1)];
			return (byte)(_rnd.Next(maxExisting+1, Math.Min(maxExisting+1, RfADdressUpperBound)));
		}

	    #endregion

		public byte FindAvailableRfAddress()
		{
			var findValidCandidate = false;
			var iteration = 0;
			byte candidate = 0;
			
			while (!findValidCandidate && iteration < 5)
			{
				candidate = FindRfAddressInternal();
				lock (typeof(FindHoleRfAddressStrategy))
				{
					findValidCandidate = !_exisitingRfAddresses.Contains(candidate);
					if (findValidCandidate)
						_exisitingRfAddresses.Add(candidate);
				}

				iteration++;
			}

			if(findValidCandidate)
				return candidate;
			throw new ApplicationException($"Could not find an available Rf Address after {iteration} iterations.");
		}
	}
}

