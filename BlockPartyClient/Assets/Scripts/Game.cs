using UnityEngine;
using System.Collections;
using BlockPartyShared;

public class Game : MonoBehaviour {
	public enum GameState
	{
		Countdown,
		Gameplay,
		Loss,
	}

	public GameState State;
	public Score Score;

	bool serverGameStateChanged;

	// Use this for initialization
	void Start () {
		if(!NetworkingManager.Connected)
		{
			NetworkingManager.Connect();
		}

		NetworkingManager.MessageReceived += networkingManager_MessageReceived;

		State = GameState.Gameplay;
	}

	void networkingManager_MessageReceived (object sender, BlockPartyShared.MessageReceivedEventArgs e)
	{
		if(e.Message.Type == BlockPartyShared.NetworkMessage.MessageType.ServerGameState &&
		   (string)e.Message.Content == "Results")
		{
			NetworkingManager.Send(new NetworkMessage(NetworkMessage.MessageType.ClientResults, Score.GameScore));
			ScoreManager.LatestScore = Score.GameScore;
			serverGameStateChanged = true;
		}
	}

	public void Lose()
	{
		State = GameState.Loss;
	}

	// Update is called once per frame
	void Update () {
		if(serverGameStateChanged)
		{
			NetworkingManager.MessageReceived -= networkingManager_MessageReceived;
			Application.LoadLevel("Results");
		}
	}

	void OnApplicationQuit()
	{
		NetworkingManager.Disconnect();
	}
}
