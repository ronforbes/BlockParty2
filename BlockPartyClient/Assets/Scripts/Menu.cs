using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(!UserManager.Initialized)
		{
			UserManager.Initialize();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(!UserManager.LoggedIn)
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 50.0f, Screen.height / 2 - 50.0f, 100.0f, 100.0f), "Login"))
			{
				UserManager.Login();
			}
		}
		else
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 50.0f, Screen.height / 2 - 50.0f, 100.0f, 100.0f), "Play"))
			{
				Application.LoadLevel("Lobby");
			}
		}
	}
}
