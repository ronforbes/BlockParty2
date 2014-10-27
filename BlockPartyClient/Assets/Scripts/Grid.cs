using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public const int Width = 6;
	public const int Height = 45;
	public const int SafeHeight = 12;
	public const int Size = Width * Height;
	public BlockManager BlockManager;

	// Use this for initialization
	void Start () {
	
	}

	public void Initialize()
	{
		BlockManager.CreateBlock(0, 0, 0);
		BlockManager.CreateBlock(1, 0, 1);
		BlockManager.CreateBlock(2, 0, 2);
		BlockManager.CreateBlock(3, 0, 3);
		BlockManager.CreateBlock(4, 0, 4);
		BlockManager.CreateBlock(5, 0, 0);
		BlockManager.CreateBlock(0, 1, 1);
		BlockManager.CreateBlock(0, 2, 2);
		BlockManager.CreateBlock(0, 3, 3);
		BlockManager.CreateBlock(0, 4, 4);
		BlockManager.CreateBlock(0, 5, 0);
		BlockManager.CreateBlock(0, 6, 1);
		BlockManager.CreateBlock(0, 7, 2);
		BlockManager.CreateBlock(0, 8, 3);
		BlockManager.CreateBlock(0, 9, 4);
		BlockManager.CreateBlock(0, 10, 0);
		BlockManager.CreateBlock(0, 11, 1);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
