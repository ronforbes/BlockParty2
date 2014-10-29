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
	BlockManager blockManager;
	Grid grid;
	BlockRaiser blockRaiser;

	public BlockSlider.SlideDirection Direction;
	public bool SlideFront;

	bool startedFalling;
	public float FallElapsed;
	public const float FallDuration = 0.1f;

	public float DieElapsed;
	public const float DieDuration = 1.5f;
	public Vector2 DyingAxis;

	public Chain Chain;
    
    // Use this for initialization
	void Start ()
	{
	
	}

	public void Initialize(int x, int y, int type)
	{
		X = x;
		Y = y;
		Type = type;

		blockManager = GameObject.Find("Game").GetComponent<BlockManager>();
		grid = GameObject.Find("Game").GetComponent<Grid>();
		blockRaiser = GameObject.Find("Game").GetComponent<BlockRaiser>();

		grid.AddBlock(x, y, this, GridElement.ElementState.Block);
	}

	public void StartSliding(BlockSlider.SlideDirection direction, bool slideFront)
	{
		State = BlockState.Sliding;
		
		Direction = direction;
		
		SlideFront = slideFront;
		
		grid.ChangeState(X, Y, this, GridElement.ElementState.Immutable);
	}
	
	public void FinishSliding(int slideX)
	{
		State = BlockState.Idle;
		
		Direction = BlockSlider.SlideDirection.None;
		
        X = slideX;
        
        grid.AddBlock(X, Y, this, GridElement.ElementState.Block);
    }

	public void StartFalling(Chain chain = null)
	{
		if (State != BlockState.Idle)
			return;
		
		// signal the falling state
		startedFalling = true;
		
		FallElapsed = FallDuration;
		
		grid.ChangeState(X, Y, this, GridElement.ElementState.Falling);
		
		if (chain != null)
		{
			BeginChainInvolvement(chain);
		}
		
		if (Y < Grid.Height - 1)
		{
			if (grid.StateAt(X, Y + 1) == GridElement.ElementState.Block)
				grid.BlockAt(X, Y + 1).StartFalling(Chain);
        }
    }

	public void StartDying(Chain chain)
	{
		// change the game state
		blockRaiser.DyingBlockCount++;
		
		BeginChainInvolvement(chain);
		
		State = BlockState.Dying;
		DieElapsed = 0.0f;
		
		grid.ChangeState(X, Y, this, GridElement.ElementState.Immutable);
		
		DyingAxis = Random.insideUnitCircle;
    }

	public void BeginChainInvolvement(Chain chain)
	{
		if (Chain != null)
		{
			Chain.DecrementInvolvement();
		}
		
		Chain = chain;
		Chain.IncrementInvolvement();
	}
	
	public void EndChainInvolvement(Chain chain)
	{
		if (Chain != null && Chain == chain)
		{
			Chain.DecrementInvolvement();
			Chain = null;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		// don't update the creep row
        if (Y == 0)
            return;
        
        if (startedFalling)
		{
			State = BlockState.Falling;
			
			startedFalling = false;
        }

		switch (State)
		{
		case BlockState.Idle:
			// we may have to fall
			if (grid.StateAt(X, Y - 1) == GridElement.ElementState.Empty)
				StartFalling();
			break;
			
		case BlockState.Falling:
			FallElapsed += Time.deltaTime;
			
			if (FallElapsed >= FallDuration)
			{
				if (grid.StateAt(X, Y - 1) == GridElement.ElementState.Empty)
				{
					// shift our grid position down to the next row
					Y--;
					FallElapsed = 0.0f;
					
					grid.Remove(X, Y + 1, this);
					grid.AddBlock(X, Y, this, GridElement.ElementState.Falling);
				}
				else
				{
					// we've landed
					
					// change our state
					State = BlockState.Idle;
					
					// update the grid
					grid.ChangeState(X, Y, this, GridElement.ElementState.Block);
					
					// register for elimination checking
					grid.RequestMatchCheck(this);
				}
			}
			break;
		case BlockState.Dying:
			DieElapsed += Time.deltaTime;
			
			if (DieElapsed >= DieDuration)
			{
				// change the game state
				blockRaiser.DyingBlockCount--;
				
				// update the grid
				grid.Remove(X, Y, this);
				
				// tell our upward neighbor to fall
				if (Y < Grid.Height - 1)
				{
					if (grid.StateAt(X, Y + 1) == GridElement.ElementState.Block)
						grid.BlockAt(X, Y + 1).StartFalling(Chain);
				}
				
				Chain.DecrementInvolvement();
				
				//ParticleManager particleManager = FindObjectOfType<ParticleManager>();
                //particleManager.CreateParticles(X, Y, Chain.Magnitude, Type);
                    
                blockManager.DeleteBlock(this);
            }
            break;
        }
    }
}
