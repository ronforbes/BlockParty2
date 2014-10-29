using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridElement
{
	public enum ElementState
	{
		Empty,
		Block,
		Immutable,
		Falling,
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

public class MatchCheck
{
	public Block Block;
	//public Chain Chain;
	
	public MatchCheck(Block block/*, Chain chain*/)
	{
		Block = block;
		//Chain = chain;
	}
}

public class Grid : MonoBehaviour {
	public const int Width = 6;
	public const int Height = 45;
	public const int SafeHeight = 11;
	public const int Size = Width * Height;
	public const int MinimumMatchLength = 3;
	public BlockManager BlockManager;
	GridElement[,] grid = new GridElement[Grid.Width, Grid.Height];
	List<MatchCheck> matchChecks = new List<MatchCheck>(BlockManager.BlockCapacity);
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
					BlockManager.SecondToLastNewRowTypes[x] = type;
				
				if (y == 1)
					BlockManager.LastNewRowTypes[x] = type;
                
                // create the block
                BlockManager.CreateBlock(x, y, type);
            }
        }
        
        topEffectiveRow = topOccupiedRow;
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
	
	public void Remove(int x, int y, Block block)
	{
		DebugUtilities.Assert(grid[x, y].Element == block);
		
		grid[x, y].Element = null;
		grid[x, y].Type = GridElement.ElementType.Empty;
        grid[x, y].State = GridElement.ElementState.Empty;
    }

    public GridElement.ElementState StateAt(int x, int y)
	{
		return grid[x, y].State;
	}

	public void ChangeState(int x, int y, Object element, GridElement.ElementState state)
	{
		DebugUtilities.Assert(grid[x, y].Element == element);
		
		grid[x, y].State = state;
    }

    public Block BlockAt(int x, int y)
	{
		DebugUtilities.Assert(grid[x, y].Type == GridElement.ElementType.Block);
		
		return grid[x, y].Element as Block;
    }

	public bool MatchAt(int x, int y, Block block)
	{
		DebugUtilities.Assert(grid[x, y].State == GridElement.ElementState.Block);
		
		return BlockManager.Match(block, grid[x, y].Element as Block);
    }

	public void RequestMatchCheck(Block block/*, Chain chain = null*/)
	{
		matchChecks.Add(new MatchCheck(block/*, chain*/));
	}
	
    // Update is called once per frame
    void Update () {
		// process elimination check requests
		while (matchChecks.Count > 0)
		{
			MatchCheck check = matchChecks[0];
			
			// ensure that the block is still static
			if (check.Block.State != Block.BlockState.Idle)
				continue;
			
			// use the block's combo, if it has one
			CheckMatch(check.Block/*, check.Block.Chain != null ? check.Block.Chain : check.Chain*/);
			
			matchChecks.Remove(check);
		}
		
		// update top occupied row
		topOccupiedRow++;
		bool flag = true;
		do
		{
			topOccupiedRow--;
			for (int x = 0; x < Grid.Width; x++)
			{
				if (StateAt(x, topOccupiedRow) != GridElement.ElementState.Empty)
				{
					flag = false;
					break;
				}
			}
		} while(flag);
		
		// update top effective row
		topEffectiveRow++;
		flag = true;
		do
		{
			topEffectiveRow--;
			for (int x = 0; x < Grid.Width; x++)
			{
				if (grid[x, topEffectiveRow].Type != GridElement.ElementType.Empty)
				{
					flag = false;
					break;
                }
            }
        } while(flag);
    }

	void CheckMatch(Block block/*, Chain chain*/)
	{
		int x = block.X;
		int y = block.Y;
		
		// look in four directions for matching lines
		
		int left = x;
		while (left > 0)
		{
			if (StateAt(left - 1, y) != GridElement.ElementState.Block)
				break;
			if (!MatchAt(left - 1, y, block))
				break;
			left--;
		}
		
		int right = x + 1;
		while (right < Width)
		{
			if (StateAt(right, y) != GridElement.ElementState.Block)
				break;
			if (!MatchAt(right, y, block))
				break;
			right++;
		}
		
		int bottom = y;
		while (bottom > 1)
		{
			if (StateAt(x, bottom - 1) != GridElement.ElementState.Block)
				break;
			if (!MatchAt(x, bottom - 1, block))
				break;
			bottom--;
		}
		
		int top = y + 1;
		while (top < Height)
		{
			if (StateAt(x, top) != GridElement.ElementState.Block)
				break;
			if (!MatchAt(x, top, block))
				break;
			top++;
		}
		
		int width = right - left;
		int height = top - bottom;
		int magnitude = 0;
		bool horizontalPattern = false;
		bool verticalPattern = false;
		
		if (width >= MinimumMatchLength)
		{
			horizontalPattern = true;
			magnitude += width;
		}
		
		if (height >= MinimumMatchLength)
		{
			verticalPattern = true;
			magnitude += height;
		}
		
		if (!horizontalPattern && !verticalPattern)
		{
			//block.EndChainInvolvement(chain);
			return;
		}
		
		/*if (chain == null)
		{
			chain = ChainManager.CreateChain();
		}*/
		
		// if pattern matches both directions
		if (horizontalPattern && verticalPattern)
			magnitude--;
		
		// kill the pattern's blocks and look for touching garbage
		block.StartDying(/*chain*/);
		
		if (horizontalPattern)
		{
			// kill the pattern's blocks
			for (int killX = left; killX < right; killX++)
			{
				if (killX != x)
				{
					BlockAt(killX, y).StartDying(/*chain*/);
				}
			}
		}
		
		if (verticalPattern)
		{
			// kill the pattern's blocks
			for (int killY = bottom; killY < top; killY++)
			{
				if (killY != y)
				{
					BlockAt(x, killY).StartDying(/*chain*/);
                }
            }
        }
        
        //chain.ReportMatch(magnitude, block);
    }

	public bool ShiftUp()
	{
		if (topOccupiedRow == Height - 1)
			return false;
		
		// shift the grid
		for (int y = topOccupiedRow + 1; y >= 0; y--)
		{
			for (int x = 0; x < Width; x++)
			{
				grid[x, y + 1].Element = grid[x, y].Element;
				grid[x, y + 1].Type = grid[x, y].Type;
				grid[x, y + 1].State = grid[x, y].State;
			}
		}
		
		// otherwise the assert will tag us
		for (int x = 0; x < Width; x++)
		{
			grid[x, 0].State = GridElement.ElementState.Empty;
		}
		
		topOccupiedRow++;
		topEffectiveRow++;
		
		BlockManager.ShiftUp();
        
        return true;
    }

	public bool IsMaxHeightReached()
	{
		return topEffectiveRow >= SafeHeight - 1;
	}
}
