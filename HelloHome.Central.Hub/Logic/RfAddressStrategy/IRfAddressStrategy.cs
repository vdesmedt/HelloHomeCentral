using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HelloHome.Central.Hub.Logic.RfAddressStrategy
{
	public interface IRfAddressStrategy
	{
		byte FindAvailableRfAddress ();
	}
}

