using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour {
	public Block BlockPrefab;
	public List<Block> Blocks = new List<Block>(BlockCapacity);
	public const int BlockCapacity = Grid.Size;
	public List<int> LastNewRowTypes = new List<int>(Grid.Width);
	public List<int> SecondToLastNewRowTypes = new List<int>(Grid.Width);
	int lastNewBlockType = 0, secondToLastNewBlockType = 0;

	// Use this for initialization
	void Start () {
		for (int x = 0; x < Grid.Width; x++)
		{
			LastNewRowTypes.Add(0);
			SecondToLastNewRowTypes.Add(0);
        }
    }
    
    public void CreateBlock(int x, int y, int type)
	{
		if(Blocks.Count == BlockCapacity)
			return;

		Block block = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity) as Block;
		block.transform.parent = transform;
		Blocks.Add(block);

		block.Initialize(x, y, type);
	}

    public void CreateNewRow()
	{
		for (int x = 0; x < Grid.Width; x++)
		{
			int type = 0;
			
			if (LastNewRowTypes.Count == 0)
				LastNewRowTypes = new List<int>(Grid.Width);
			if (SecondToLastNewRowTypes.Count == 0)
				SecondToLastNewRowTypes = new List<int>(Grid.Width);
			
			do
			{
				type = Random.Range(0, Block.TypeCount);
			} while((type == lastNewBlockType && lastNewBlockType == secondToLastNewBlockType) ||
			        (type == LastNewRowTypes[x] && LastNewRowTypes[x] == SecondToLastNewRowTypes[x]));
			
			SecondToLastNewRowTypes[x] = LastNewRowTypes[x];
			LastNewRowTypes[x] = type;
			
			secondToLastNewBlockType = lastNewBlockType;
            lastNewBlockType = type;
            
            CreateBlock(x, 0, type);
        }
    }
    
	public void DeleteBlock(Block block)
	{
		Blocks.Remove(block);
		Destroy(block.gameObject);
    }

    public void ShiftUp()
	{
		foreach (Block block in Blocks)
		{
			block.Y++;
		}
	}

    public bool Match(Block block1, Block block2)
	{
		return block1.Type == block2.Type;
	}

    // Update is called once per frame
    void Update () {
	
	}
}
