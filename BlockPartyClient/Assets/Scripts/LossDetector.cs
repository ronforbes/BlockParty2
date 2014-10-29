using UnityEngine;
using System.Collections;

public class LossDetector : MonoBehaviour {
	public BlockRaiser BlockRaiser;
	public Grid Grid;
	public Game Game;
	bool maxHeightReached;
	float lossElapsed;
	const float lossDuration = 3.0f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if(BlockRaiser.DyingBlockCount != 0)
		{
			return;
		}

		if(maxHeightReached)
		{
			if (!Grid.IsMaximumHeightReached())
				maxHeightReached = false;
			else
			{
				lossElapsed += Time.deltaTime;
				
				if (lossElapsed >= lossDuration)
					Game.Lose();
				
				return;
			}
		}
		else
		{
			if (Grid.IsMaximumHeightReached())
			{
				maxHeightReached = true;
				lossElapsed = 0.0f;
			}
		}
	}
}
