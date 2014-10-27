using UnityEngine;
using System.Collections;

public class BlockDisplayer : MonoBehaviour {
	public Block Block;
	Color[] blockColors = new Color[Block.TypeCount];
	Color[] creepColors = new Color[Block.TypeCount];
	const float gridElementLength = 1.0f;

	// Use this for initialization
	void Start () {
		blockColors[0] = new Color(0.73f, 0.0f, 0.73f);
		blockColors[1] = new Color(0.2f, 0.2f, 0.8f);
		blockColors[2] = new Color(0.0f, 0.6f, 0.05f);
		blockColors[3] = new Color(0.85f, 0.85f, 0.0f);
		blockColors[4] = new Color(1.0f, 0.4f, 0.0f);

		creepColors[0] = new Color(0.25f * 0.73f, 0.25f * 0.0f, 0.25f * 0.73f);
		creepColors[1] = new Color(0.25f * 0.2f, 0.25f * 0.2f, 0.25f * 0.8f);
		creepColors[2] = new Color(0.25f * 0.0f, 0.25f * 0.6f, 0.25f * 0.05f);
		creepColors[3] = new Color(0.25f * 0.85f, 0.25f * 0.85f, 0.25f * 0.0f);
		creepColors[4] = new Color(0.25f * 1.0f, 0.25f * 0.4f, 0.25f * 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		float x, y;

		x = Block.X * gridElementLength;
		y = Block.Y * gridElementLength;

		if (Block.Y != 0)
			Block.transform.Find("Cube").renderer.material.color = blockColors[Block.Type];
		else
			Block.transform.Find("Cube").renderer.material.color = creepColors[Block.Type];
		
		Block.transform.position = new Vector3(x, y, 0.0f);
		Block.transform.rotation = Quaternion.identity;
	}
}
