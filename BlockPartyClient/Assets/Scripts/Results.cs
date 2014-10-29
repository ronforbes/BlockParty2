using UnityEngine;
using System.Collections;

public class Results : MonoBehaviour {
	bool serverGameStateChanged;

	// Use this for initialization
	void Start () {
		if(!NetworkingManager.Connected)
		{
			NetworkingManager.Connect();
		}

		NetworkingManager.MessageReceived += networkingManager_MessageReceived;
	}

	void networkingManager_MessageReceived (object sender, BlockPartyShared.MessageReceivedEventArgs e)
	{
		if(e.Message.Type == BlockPartyShared.NetworkMessage.MessageType.ServerGameState &&
		   (string)e.Message.Content == "Lobby")
		{
			serverGameStateChanged = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(serverGameStateChanged)
		{
			Application.LoadLevel("Lobby");
		}
	}

	void OnApplicationQuit()
	{
		NetworkingManager.Disconnect();
	}
}
