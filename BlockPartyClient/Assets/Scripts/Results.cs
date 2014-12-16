using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Results : MonoBehaviour {
	public GUIText ScoreText;

	bool serverGameStateChanged;

	// Use this for initialization
	void Start () {
		if(!NetworkingManager.Connected)
		{
			NetworkingManager.Connect();
		}

		NetworkingManager.MessageReceived += networkingManager_MessageReceived;

		ScoreText.text = ScoreManager.LatestScore.ToString();
	}

	void networkingManager_MessageReceived (object sender, BlockPartyShared.MessageReceivedEventArgs e)
	{
		if(e.Message.Type == BlockPartyShared.NetworkMessage.MessageType.ServerGameState &&
		   (string)e.Message.Content == "Lobby")
		{
			serverGameStateChanged = true;
		}

		if(e.Message.Type == BlockPartyShared.NetworkMessage.MessageType.ServerLeaderboard)
		{
			ScoreManager.SortedGameResults = (List<KeyValuePair<string, int>>)e.Message.Content;
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
