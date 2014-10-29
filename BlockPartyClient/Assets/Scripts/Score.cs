using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	public int GameScore;

	// Use this for initialization
	void Start () {
	
	}

	public void ReportChain(Chain chain)
	{
		GameScore += chain.Magnitude * 100;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
