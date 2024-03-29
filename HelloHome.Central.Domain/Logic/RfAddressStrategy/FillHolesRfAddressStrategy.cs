﻿using System;
using System.Collections.Generic;
using System.Linq;
using HelloHome.Central.Common.Exceptions;
using HelloHome.Central.Domain.CmdQrys;
using NLog;

namespace HelloHome.Central.Domain.Logic.RfAddressStrategy
{
	public class FillHolesRfAddressStrategy : IRfAddressStrategy
	{
		private static readonly Logger Logger = LogManager.GetLogger(nameof(FillHolesRfAddressStrategy));		

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
			var holes = Enumerable.Range(RfAddressLowerBound, RfAddressUpperBound).Where (i => !_exisitingRfAddresses.Contains(i)).ToList ();
			if (holes.Any()) 
				return holes [_rnd.Next(holes.Count - 1)];
			throw new ApplicationException("No more addresses available");
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

