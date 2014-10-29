using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width / 2 - 50.0f, Screen.height / 2 - 50.0f, 100.0f, 100.0f), "Play"))
		{
			Application.LoadLevel("Game");
		}
	}
}
