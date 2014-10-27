using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	public enum BlockState
	{
		Idle,
		Sliding,
	}

	public int X, Y;
	public int Type;
	public const int TypeCount = 5;
	public BlockState State;
	BlockManager blockManager;
	Grid grid;
	public BlockController.SlideDirection Direction;
	public bool SlideFront;
    
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

		grid.AddBlock(x, y, this, GridElement.ElementState.Block);
	}

	public void StartSliding(BlockController.SlideDirection direction, bool slideFront)
	{
		State = BlockState.Sliding;
		
		Direction = direction;
		
		SlideFront = slideFront;
		
		grid.ChangeState(X, Y, this, GridElement.ElementState.Immutable);
	}
	
	public void FinishSliding(int slideX)
	{
		State = BlockState.Idle;
		
		Direction = BlockController.SlideDirection.None;
		
        X = slideX;
        
        grid.AddBlock(X, Y, this, GridElement.ElementState.Block);
    }

    // Update is called once per frame
	void Update ()
	{
	
	}
}
