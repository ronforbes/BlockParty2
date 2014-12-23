using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {
	bool serverGameStateChanged;
	public GUIText UserText;
	public GUITexture UserPictureTexture;
	public GUIText RankText;
	public GUIText NameText;
	public GUIText ScoreText;

	// Use this for initialization
	void Start () {
		if(!NetworkingManager.Connected)
		{
			NetworkingManager.Connect();
		}
		
		NetworkingManager.MessageReceived += networkingManager_MessageReceived;

		UserText.text = UserManager.Name;
		StartCoroutine("LoadProfilePicture", UserPictureTexture);
	}
	
	void networkingManager_MessageReceived (object sender, BlockPartyShared.MessageReceivedEventArgs e)
	{
		if(e.Message.Type == BlockPartyShared.NetworkMessage.MessageType.ServerGameState &&
		   (string)e.Message.Content == "Game")
		{
			serverGameStateChanged = true;
		}

		if(e.Message.Type == BlockPartyShared.NetworkMessage.MessageType.ServerLeaderboard)
		{
			ScoreManager.SortedGameResults = (List<KeyValuePair<string, int>>)e.Message.Content;
		}
	}

	IEnumerator LoadProfilePicture(GUITexture guiTexture)
	{
		WWW www = new WWW("https://graph.facebook.com/me/picture?access_token=" + FB.AccessToken);
		yield return www;
		Debug.Log(www.bytes);
		Texture2D texture = new Texture2D(128, 128, TextureFormat.DXT1, false);
		guiTexture.texture = texture;
		www.LoadImageIntoTexture(texture);
	}

	// Update is called once per frame
	void Update () {
		if(ScoreManager.SortedGameResults != null)
		{
			RankText.text = "";
			NameText.text = "";
			ScoreText.text = "";

			int rank = 1;
			foreach(KeyValuePair<string, int> pair in ScoreManager.SortedGameResults)
			{
				RankText.text += rank.ToString() + "\n";
				NameText.text += pair.Key + "\n";
				ScoreText.text += pair.Value + "\n";
				
				rank++;
			}
		}

		if(serverGameStateChanged)
		{
			NetworkingManager.MessageReceived -= networkingManager_MessageReceived;
			ScoreManager.SortedGameResults = null;
			Application.LoadLevel("Game");
		}
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(10.0f, 10.0f, 50.0f, 50.0f), "Back"))
		{
			NetworkingManager.MessageReceived -= networkingManager_MessageReceived;
			Application.LoadLevel("Menu");
		}
	}

	void OnApplicationQuit()
	{
		NetworkingManager.Disconnect();
	}
}
