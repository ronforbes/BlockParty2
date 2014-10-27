using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour {
	public Block BlockPrefab;
	public List<Block> Blocks = new List<Block>(BlockCapacity);
	public const int BlockCapacity = Grid.Size;
	public List<int> LastRowCreepTypes = new List<int>(Grid.Width);
	public List<int> SecondToLastRowCreepTypes = new List<int>(Grid.Width);
	int lastCreepType, secondToLastCreepType;

	// Use this for initialization
	void Start () {
		lastCreepType = secondToLastCreepType = 0;
		
		for (int x = 0; x < Grid.Width; x++)
		{
			LastRowCreepTypes.Add(0);
			SecondToLastRowCreepTypes.Add(0);
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

	public void CreateCreepRow()
	{
		for (int x = 0; x < Grid.Width; x++)
		{
			int type = 0;
			
			if (LastRowCreepTypes.Count == 0)
				LastRowCreepTypes = new List<int>(Grid.Width);
			if (SecondToLastRowCreepTypes.Count == 0)
				SecondToLastRowCreepTypes = new List<int>(Grid.Width);
			
			do
			{
				type = Random.Range(0, Block.TypeCount);
			} while((type == lastCreepType && lastCreepType == secondToLastCreepType) ||
			        (type == LastRowCreepTypes[x] && LastRowCreepTypes[x] == SecondToLastRowCreepTypes[x]));
			
			SecondToLastRowCreepTypes[x] = LastRowCreepTypes[x];
			LastRowCreepTypes[x] = type;
			
			secondToLastCreepType = lastCreepType;
            lastCreepType = type;
            
            CreateBlock(x, 0, type);
        }
    }
    
	public bool Match(Block block1, Block block2)
	{
		return block1.Type == block2.Type;
	}

    public void DeleteBlock(Block block)
	{
		Blocks.Remove(block);
		Destroy(block.gameObject);
	}

    // Update is called once per frame
    void Update () {
	
	}
}
