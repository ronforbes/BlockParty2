using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	public enum BlockState
	{
		Idle,
		Sliding,
		Falling,
		Dying,
	}

	public int X, Y;
	public int Type;
	public const int TypeCount = 5;
	public BlockState State;
	Grid grid;
	
    // Use this for initialization
	void Start ()
	{
	
	}

	public void Initialize(int x, int y, int type)
	{
		X = x;
		Y = y;
		Type = type;

		GameObject game = GameObject.Find("Game");
		if(game != null)
		{
			grid = game.GetComponent<Grid>();
		}

		grid.AddBlock(x, y, this, GridElement.ElementState.Block);
	}

	// Update is called once per frame
	void Update ()
	{
		// don't update the creep row
        if (Y == 0)
            return;

		if(State == BlockState.Idle)
		{
			// we may have to fall
			if (grid.StateAt(X, Y - 1) == GridElement.ElementState.Empty)
			{
				GetComponent<BlockFalling>().StartFalling();
			}
        }
    }
}
