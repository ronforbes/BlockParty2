using UnityEngine;
using System.Collections;

public class GridElement
{
	public enum ElementState
	{
		Empty,
		Block,
	}

	public enum ElementType
	{
		Empty,
		Block,
	}

	public ElementState State;
	public ElementType Type;
	public Object Element;
}

public class Grid : MonoBehaviour {
	public const int Width = 6;
	public const int Height = 45;
	public const int SafeHeight = 12;
	public const int Size = Width * Height;
	public BlockManager BlockManager;
	GridElement[,] grid = new GridElement[Grid.Width, Grid.Height];
	int topOccupiedRow = 0;
	int topEffectiveRow = 0;

	// Use this for initialization
	void Start () {
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				grid[x, y] = new GridElement();
				grid[x, y].State = GridElement.ElementState.Empty;
				grid[x, y].Type = GridElement.ElementType.Empty;
				grid[x, y].Element = null;
			}
		}
		
		int shortColumn = Random.Range(0, Width);
		
		for (int x = Width - 1; x >= 0; x--)
		{
			int height = (shortColumn == x ? 2 : 7) + Random.Range(0, 2);
			
			if (height - 1 > topOccupiedRow)
				topOccupiedRow = height - 1;
			
			for (int y = height - 1; y >= 1; y--)
			{
				int type;
				do
				{
					type = Random.Range(0, Block.TypeCount);
					
					if (!(StateAt(x, y + 1) == GridElement.ElementState.Empty) &&
					    BlockAt(x, y + 1).Type == type)
						continue;
					
					if (x == Grid.Width - 1)
						break;
					
					if (!(StateAt(x + 1, y) == GridElement.ElementState.Empty) &&
					    BlockAt(x + 1, y).Type == type)
						continue;
					
					break;
				} while (true);
				
				// setup creep creation state
				if (y == 2)
					BlockManager.SecondToLastRowCreepTypes[x] = type;
				
				if (y == 1)
					BlockManager.LastRowCreepTypes[x] = type;
                
                // create the block
                BlockManager.CreateBlock(x, y, type);
            }
        }
        
        topEffectiveRow = topOccupiedRow;
    }

	public GridElement.ElementState StateAt(int x, int y)
	{
		return grid[x, y].State;
	}
	
	public Block BlockAt(int x, int y)
	{
		DebugUtilities.Assert(grid[x, y].Type == GridElement.ElementType.Block);
		
		return grid[x, y].Element as Block;
    }

	public void AddBlock(int x, int y, Block block, GridElement.ElementState state)
	{
		DebugUtilities.Assert(x < Width);
		DebugUtilities.Assert(y < Height);
		DebugUtilities.Assert(grid[x, y].State == GridElement.ElementState.Empty);
		
		grid[x, y].Element = block;
		grid[x, y].Type = GridElement.ElementType.Block;
		grid[x, y].State = state;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
