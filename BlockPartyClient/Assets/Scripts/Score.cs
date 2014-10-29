using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	public int GameScore;
	const int chainMultiplier = 100;

	// Use this for initialization
	void Start () {
	
	}

	public void ReportChain(Chain chain)
	{
		GameScore += chain.Magnitude * chainMultiplier;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
