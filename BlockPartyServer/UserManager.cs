using System.Collections.Generic;
using System.Net.Sockets;

namespace BlockPartyServer
{
	public class UserManager
	{
		public Dictionary<string, User> Users = new Dictionary<string, User>();

		public User GetUserByTcpClient(TcpClient client)
		{
			foreach(KeyValuePair<string, User> pair in Users)
			{
				if(pair.Value.Client == client)
					return Users[pair.Key];
			}

			return null;
		}
	}
}