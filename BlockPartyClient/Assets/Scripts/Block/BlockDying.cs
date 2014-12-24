using UnityEngine;
using System.Collections;

public class BlockDying : MonoBehaviour {
	public float DieElapsed;
	public const float DieDuration = 1.5f;
	public Vector2 DyingAxis;
	Block block;
	BlockManager blockManager;
	Grid grid;
	BlockRaiser blockRaiser;

	// Use this for initialization
	void Start () {
		block = GetComponent<Block>();

		GameObject game = GameObject.Find("Game");
		if(game != null)
		{
			blockManager = game.GetComponent<BlockManager>();
			grid = game.GetComponent<Grid>();
			blockRaiser = game.GetComponent<BlockRaiser>();
		}
	}

	public void StartDying(Chain chain)
	{
		// change the game state
		blockRaiser.DyingBlockCount++;

		GetComponent<BlockChaining>().BeginChainInvolvement(chain);

		block.State = Block.BlockState.Dying;
		DieElapsed = 0.0f;
		
		grid.ChangeState(block.X, block.Y, this, GridElement.ElementState.Immutable);
		
		DyingAxis = Random.insideUnitCircle;
	}

	// Update is called once per frame
	void Update () {
		if(block.State == Block.BlockState.Dying)
		{
			DieElapsed += Time.deltaTime;
			
			if (DieElapsed >= DieDuration)
			{
				// change the game state
				blockRaiser.DyingBlockCount--;
				
				// update the grid
				grid.Remove(block.X, block.Y, block);
				
				// tell our upward neighbor to fall
				if (block.Y < Grid.Height - 1)
				{
					if (grid.StateAt(block.X, block.Y + 1) == GridElement.ElementState.Block)
						grid.BlockAt(block.X, block.Y + 1).GetComponent<BlockFalling>().StartFalling(GetComponent<BlockChaining>().Chain);
				}
				
				GetComponent<BlockChaining>().Chain.DecrementInvolvement();
				
				//ParticleManager particleManager = FindObjectOfType<ParticleManager>();
				//particleManager.CreateParticles(X, Y, Chain.Magnitude, Type);
				
				blockManager.DeleteBlock(block);
			}
		}
	}
}
