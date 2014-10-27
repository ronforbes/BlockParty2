﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public enum GameState
	{
		Countdown,
		Gameplay,
		Loss,
	}

	public GameState State;

	// Use this for initialization
	void Start () {
		State = GameState.Gameplay;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
