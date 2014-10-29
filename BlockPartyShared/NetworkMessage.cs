using System;

namespace BlockPartyShared
{
	[Serializable]
	public class NetworkMessage
	{
		public enum MessageType
		{
			ServerGameState,
			ServerLeaderboard,
			ClientResults
		}
		
		public MessageType Type;
		public object Content;
		
		public NetworkMessage(MessageType type, object content)
		{
			Type = type;
			Content = content;
		}
		
		public override string ToString()
		{
			return string.Format("[NetworkMessage: Type={0}, Content={1}]", Type.ToString(), Content.ToString());
		}
	}
	
	public class MessageReceivedEventArgs : EventArgs
	{
		public NetworkMessage Message { get; set; }
		
		public string Sender { get; set; }
	}
}

