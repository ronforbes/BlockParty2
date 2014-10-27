using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	public enum BlockState
	{
		Idle,
	}

	public int X, Y;
	public int Type;
	public const int TypeCount = 5;
	public BlockState State;
	BlockManager blockManager;
	Grid grid;

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

	// Update is called once per frame
	void Update ()
	{
	
	}
}
