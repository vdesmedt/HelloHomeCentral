using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HelloHome.Central.Common.Configuration;
using HelloHome.Central.Common.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HelloHome.Central.Hub.Agents.EmonCms
{
	public class EmonCmsAgent : IEmonCmsAgent
	{
		private const string urlBase = "http://emoncms.org/input/post.json?";

		EmonCmsConfig config;

		public EmonCmsAgent (IOptionsMonitor<EmonCmsConfig> config)
		{
			this.config = config.CurrentValue;
			
		}

		public void Send<T>(T data) {
			var json = "json=" + JsonConvert.SerializeObject (data);
			SendInternal (json);
		}

		public void Send<T>(int nodeId, IList<T> values, DateTime? timestamp = null) where T : struct
		{
			var ps = new List<string> { "node=" + nodeId, "csv=" + String.Join (",", values) };
			if (timestamp != null)
				ps.Add ("time="+timestamp.Value.ToEpoch ());
			SendInternal (ps.ToArray());
		}

		public void Send(string json){
			SendInternal ("json=" + json);
		}

		private void SendInternal(params string[] parameters) {
			var paramList = parameters.ToList ();
			paramList.Add ("apikey="+config.ApiKey);
			var url = urlBase + String.Join ("&", paramList);
			var request = WebRequest.CreateHttp(url);
			var response = request.GetResponse() as HttpWebResponse;
			using (var sr = new StreamReader (response.GetResponseStream ())) {
				var respContent = sr.ReadToEnd ();
				if(respContent != "OK"){
					
				}
			}
		}
	}
}

