using System;
using System.Collections.Generic;
using System.Linq;
using HelloHome.Central.Common.Exceptions;
using HelloHome.Central.Hub.CmdQrys;
using NLog;

namespace HelloHome.Central.Hub.Logic.RfAddressStrategy
{
	public class FillHolesRfAddressStrategy : IRfAddressStrategy
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();		

		private readonly SortedSet<int> _exisitingRfAddresses;
		private readonly Random _rnd;

		public int RfAddressLowerBound { get; set; } = 10;
		public int RfAddressUpperBound { get; set; } = (2^10)-1;

		private FillHolesRfAddressStrategy()
		{
			_rnd = new Random ();
		}

		public FillHolesRfAddressStrategy (IListRfIdsQuery listRfAddressesQuery): this()
		{
			_exisitingRfAddresses = new SortedSet<int>(listRfAddressesQuery.Execute());
		}

		public FillHolesRfAddressStrategy (IEnumerable<int> existingRfAddresses) : this()
		{
			_exisitingRfAddresses = new SortedSet<int>(existingRfAddresses);
		}

		#region IRfNodeIdGeneratorStrategy implementation

		private int FindRfAddressInternal ()
		{
			if (!_exisitingRfAddresses.Any ())
				return RfAddressLowerBound;
			var maxExisting = _exisitingRfAddresses.Max ();
			var holes = Enumerable.Range(1, maxExisting).Select(i => (byte)i).Where (i => !_exisitingRfAddresses.Contains(i)).ToList ();
			if (holes.Any()) 
				return holes [_rnd.Next(holes.Count - 1)];
			return (byte)(_rnd.Next(maxExisting+1, RfAddressUpperBound));
		}

	    #endregion

		public int FindAvailableRfAddress()
		{
			if(_exisitingRfAddresses.Count == RfAddressUpperBound)
				throw new NoAvailableRfAddressException(false);
			var findValidCandidate = false;
			var iteration = 0;
			int candidate = 0;
			
			while (!findValidCandidate && iteration < 5)
			{
				candidate = FindRfAddressInternal();
				lock (typeof(FillHolesRfAddressStrategy))
				{
					findValidCandidate = !_exisitingRfAddresses.Contains(candidate);
					if (findValidCandidate)
						_exisitingRfAddresses.Add(candidate);
				}

				iteration++;
			}

			if(findValidCandidate)
				return candidate;
			throw new NoAvailableRfAddressException(true);
		}
	}
}

