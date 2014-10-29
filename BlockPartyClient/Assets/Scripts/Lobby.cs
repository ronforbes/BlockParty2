using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {
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
		   (string)e.Message.Content == "Game")
		{
			serverGameStateChanged = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(serverGameStateChanged)
		{
			Application.LoadLevel("Game");
		}
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(10.0f, 10.0f, 50.0f, 50.0f), "Back"))
		{
			Application.LoadLevel("Menu");
		}
	}

	void OnApplicationQuit()
	{
		NetworkingManager.Disconnect();
	}
}
