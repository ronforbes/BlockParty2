using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using BlockPartyShared;
using UnityEngine;
using System.Threading;

public static class NetworkingManager
{
	static TcpClient client;
	static NetworkStream stream;
	static BinaryFormatter formatter;
	static readonly string hostname = "localhost";
	static readonly int port = 1337;

	public static event EventHandler<MessageReceivedEventArgs> MessageReceived;

	public static bool Connected
	{
		get
		{
			return client != null && client.Connected;
		}
	}

	public static void Connect()
	{
		client = new TcpClient(hostname, port);
		
		if (client.Connected)
		{
			Debug.Log("Connected to server " + client.Client.RemoteEndPoint.ToString());
			
			stream = client.GetStream();
			formatter = new BinaryFormatter();
			
			if (stream.CanRead)
			{
				Thread receiveThread = new Thread(Receive);
				receiveThread.Start();
			}
		}
	}

	public static void Disconnect()
	{
		client.Close();
		Debug.Log("Disconnected from server");
	}

	static void Receive()
	{
		while (true)
		{
			NetworkMessage message = (NetworkMessage)formatter.Deserialize(stream);
			Debug.Log("Received message from server: " + message.ToString());
			
			// process message
			MessageReceivedEventArgs args = new MessageReceivedEventArgs();
			args.Message = message;
			OnMessageReceived(args);
		}
	}
	
	static void OnMessageReceived(MessageReceivedEventArgs e)
	{
		EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
		if (handler != null)
		{
			handler(null, e);
		}
	}
	
	public static void Send(NetworkMessage message)
	{
		formatter.Serialize(stream, message);
		Debug.Log("Sent message to server: " + message.ToString());
	}
}