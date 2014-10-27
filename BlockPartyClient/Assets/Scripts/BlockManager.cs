using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour {
	public Block BlockPrefab;
	public List<Block> Blocks = new List<Block>(BlockCapacity);
	public const int BlockCapacity = Grid.Size;

	// Use this for initialization
	void Start () {
	
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

	// Update is called once per frame
	void Update () {
	
	}
}
