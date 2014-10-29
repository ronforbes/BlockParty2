using UnityEngine;
using System.Collections;

public class ScoreDisplayer : MonoBehaviour {
	public Score Score;
	public GUIText ScoreText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ScoreText.text = Score.GameScore.ToString();
	}
}
