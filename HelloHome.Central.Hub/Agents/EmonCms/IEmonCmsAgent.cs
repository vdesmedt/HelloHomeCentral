using System;
using System.Collections.Generic;

namespace HelloHome.Central.Hub.Agents.EmonCms
{
	public interface IEmonCmsAgent {
		void Send<T> (T data);
		void Send<T> (int nodeId, IList<T> values, DateTime? timestamp = null) where T : struct;
		void Send(string json);
	}
	
}
