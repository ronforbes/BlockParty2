using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {
	float splashElapsed;
	const float splashDuration = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		splashElapsed += Time.deltaTime;

		if(splashElapsed >= splashDuration)
		{
			Application.LoadLevel("Menu");
		}
	}
}
