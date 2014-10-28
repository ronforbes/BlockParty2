using UnityEngine;
using System.Collections;

public class BlockRaiser : MonoBehaviour {
	public BlockManager BlockManager;
	public Grid Grid;
	public Game Game;
	public float RaiseElapsed;
	public const float RaiseDuration = 1.0f;
	public int DyingBlockCount;
	float raiseDelayElapsed;
	float raiseDelaySpeed = 1.0f;
	const float raiseDelayDuration = 1.0f;
	float previousClickTime;
	const float doubleClickSpeed = 0.5f;
	bool advance, advanceTriggered;
	const float advanceDelaySpeed = 100.0f;

    // Use this for initialization
	void Start () {
		RaiseElapsed = 0.0f;
		raiseDelayElapsed = 0.0f;
		raiseDelaySpeed = 1.0f;
		
		BlockManager.CreateNewRow();
	}
	
	// Update is called once per frame
	void Update () {
		if (Game.State == Game.GameState.Countdown || Game.State == Game.GameState.Loss)
			return;

		if (DyingBlockCount != 0)
		{
			return;
		}

		/*if (creepFreeze)
		{
			if (!Grid.CheckSafeHeightViolation())
				creepFreeze = false;
			else
			{
				lossElapsed += Time.deltaTime;
				
				if (lossElapsed >= lossDuration)
					Round.Lose();
				
				return;
			}
		}
		else
		{
			if (Grid.CheckSafeHeightViolation())
			{
				creepFreeze = true;
				lossElapsed = 0.0f;
            }
        }*/

		if (Input.GetMouseButtonDown(0))
		{
			if (Time.time - previousClickTime <= doubleClickSpeed)
				advanceTriggered = true;
			else
				advanceTriggered = false;
			
			previousClickTime = Time.time;
		}
		else
			advanceTriggered = false;

        if (advance || advanceTriggered)
        {
            if (raiseDelaySpeed < advanceDelaySpeed)
            {
                raiseDelayElapsed += advanceDelaySpeed * Time.deltaTime;
            }
            else
            {
				raiseDelayElapsed += raiseDelaySpeed * Time.deltaTime;
			}
			
			advance = true;
		}
		else
		{
			raiseDelayElapsed += raiseDelaySpeed * Time.deltaTime;
        }

        while (raiseDelayElapsed >= raiseDelayDuration)
        {
            raiseDelayElapsed -= raiseDelayDuration;
            
            RaiseElapsed += Time.deltaTime;
			
			if (RaiseElapsed >= RaiseDuration)
			{
				RaiseElapsed = 0.0f;
				
				// shift everything up one grid row
				if (Grid.ShiftUp())
				{
					// create a new bottom creep row
					BlockManager.CreateNewRow();
					
					//link the elimination requests
					for (int x = 0; x < Grid.Width; x++)
					{
						Grid.RequestMatchCheck(Grid.BlockAt(x, 1));
					}
				}
				else
				{
					raiseDelayElapsed += raiseDelayDuration;
					RaiseElapsed = RaiseDuration - 0.1f;
				}
				
				if (advance && !advanceTriggered)
				{
					advance = false;
				}
			}
		}
	}
}
