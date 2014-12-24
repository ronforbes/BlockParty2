using UnityEngine;
using System.Collections;

public class BlockFalling : MonoBehaviour {
	public float FallElapsed;
	public const float FallDuration = 1.0f; // 0.1f
	Block block;
	Grid grid;

	// Use this for initialization
	void Start () {
		block = GetComponent<Block>();
		
		GameObject game = GameObject.Find("Game");
		if(game != null)
		{
			grid = game.GetComponent<Grid>();
		}
	}

	public void StartFalling(Chain chain = null)
	{
		if (block.State != Block.BlockState.Idle)
			return;
		
		// signal the falling state
		block.State = Block.BlockState.Falling;
		
		FallElapsed = 0.0f;
		
		grid.ChangeState(block.X, block.Y, block, GridElement.ElementState.Falling);
		
		if (chain != null)
		{
			GetComponent<BlockChaining>().BeginChainInvolvement(chain);
		}
		
		if (block.Y < Grid.Height - 1)
		{
			if (grid.StateAt(block.X, block.Y + 1) == GridElement.ElementState.Block)
				grid.BlockAt(block.X, block.Y + 1).GetComponent<BlockFalling>().StartFalling(GetComponent<BlockChaining>().Chain);
		}
	}

	// Update is called once per frame
	void Update () {
		if(block.State == Block.BlockState.Falling)
		{
			FallElapsed += Time.deltaTime;
			
			if (FallElapsed >= FallDuration)
			{
				// shift our grid position down to the next row
				block.Y--;

				if (grid.StateAt(block.X, block.Y - 1) == GridElement.ElementState.Empty)
				{
					FallElapsed = 0.0f;

					if(grid.StateAt(block.X, block.Y + 2) == GridElement.ElementState.Empty)
					{
						grid.Remove(block.X, block.Y + 1, block);
					}
					grid.AddBlock(block.X, block.Y, block, GridElement.ElementState.Falling);
				}
				else
				{
					// we've landed
					
					// change our state
					block.State = Block.BlockState.Idle;
					
					// update the grid
					if(grid.StateAt(block.X, block.Y + 2) == GridElement.ElementState.Empty)
					{
						grid.Remove(block.X, block.Y + 1, block);
					}
					grid.AddBlock(block.X, block.Y, block, GridElement.ElementState.Block);
					
					// register for elimination checking
					grid.RequestMatchCheck(block);
				}
			}
		}
	}
}
