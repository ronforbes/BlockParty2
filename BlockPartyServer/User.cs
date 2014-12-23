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
		public string Name;
		public TcpClient Client;

		public User(string name, TcpClient client)
		{
			Name = name;
			Client = client;
		}
	}
}