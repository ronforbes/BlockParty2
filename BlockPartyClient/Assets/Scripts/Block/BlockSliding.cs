using UnityEngine;
using System.Collections;

public class BlockSliding : MonoBehaviour {
	public BlockSlider.SlideDirection Direction;
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

	public void StartSliding(BlockSlider.SlideDirection direction)
	{
		block.State = Block.BlockState.Sliding;
		
		Direction = direction;
		
		grid.ChangeState(block.X, block.Y, block, GridElement.ElementState.Immutable);
	}

	public void FinishSliding(int slideX)
	{
		block.State = Block.BlockState.Idle;
		
		Direction = BlockSlider.SlideDirection.None;
		
		block.X = slideX;
		
		grid.AddBlock(block.X, block.Y, block, GridElement.ElementState.Block);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
