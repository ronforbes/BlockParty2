using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web.Script.Serialization;

namespace BlockPartyServer
{
	public class User
	{
		public string FacebookId;
		public string Name;
		public TcpClient Client;

		public User(string facebookId, TcpClient client)
		{
			FacebookId = facebookId;
			Client = client;

			// Don't forget to change this back to facebook ID once you start using real data!
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://graph.facebook.com/6207489"/* + facebookId*/);
			request.Method = "GET";
			WebResponse response = request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			string responseString = reader.ReadToEnd();
			reader.Close();
			var serializer = new JavaScriptSerializer();
			var dictionary = serializer.Deserialize<Dictionary<string, object>>(responseString);
			Name = dictionary["name"] as string;
		}
	}
}