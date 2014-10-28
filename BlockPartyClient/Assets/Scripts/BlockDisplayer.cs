using UnityEngine;
using System.Collections;

public class BlockDisplayer : MonoBehaviour {
	public Block Block;
	BlockSlider slider;
	BlockRaiser raiser;
	Color[] colors = new Color[Block.TypeCount];
	Color[] newColors = new Color[Block.TypeCount];
	const float gridElementLength = 1.0f;
	const float dyingFlashDuration = 0.2f;
	const float dyingSpeed = 1000;
	Color flashColor = new Color(1.0f, 1.0f, 1.0f);
    
    // Use this for initialization
	void Start () {
		slider = GameObject.Find("Game").GetComponent<BlockSlider>();
		raiser = GameObject.Find("Game").GetComponent<BlockRaiser>();

		colors[0] = new Color(0.73f, 0.0f, 0.73f);
		colors[1] = new Color(0.2f, 0.2f, 0.8f);
		colors[2] = new Color(0.0f, 0.6f, 0.05f);
		colors[3] = new Color(0.85f, 0.85f, 0.0f);
		colors[4] = new Color(1.0f, 0.4f, 0.0f);

		newColors[0] = new Color(0.25f * 0.73f, 0.25f * 0.0f, 0.25f * 0.73f);
		newColors[1] = new Color(0.25f * 0.2f, 0.25f * 0.2f, 0.25f * 0.8f);
		newColors[2] = new Color(0.25f * 0.0f, 0.25f * 0.6f, 0.25f * 0.05f);
		newColors[3] = new Color(0.25f * 0.85f, 0.25f * 0.85f, 0.25f * 0.0f);
		newColors[4] = new Color(0.25f * 1.0f, 0.25f * 0.4f, 0.25f * 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		float x, y;

		float raiseOffset = gridElementLength * raiser.RaiseElapsed / BlockRaiser.RaiseDuration;

		x = Block.X * gridElementLength;
		y = Block.Y * gridElementLength + raiseOffset;

		switch (Block.State)
		{
		case Block.BlockState.Idle:
			if (Block.Y != 0)
				Block.transform.Find("Cube").renderer.material.color = colors[Block.Type];
			else
				Block.transform.Find("Cube").renderer.material.color = newColors[Block.Type];
			
			Block.transform.position = new Vector3(x, y, 0.0f);
			Block.transform.rotation = Quaternion.identity;
			break;
			
		case Block.BlockState.Sliding:
			float slideOrigin = 0.0f;

			if (Block.Direction == BlockSlider.SlideDirection.Left)
			{
				slideOrigin = -gridElementLength / 2.0f;
			}
			
			if (Block.Direction == BlockSlider.SlideDirection.Right)
			{
				slideOrigin = gridElementLength / 2.0f;
			}
			
			Block.transform.Find("Cube").renderer.material.color = colors[Block.Type];
			
			Vector3 center = new Vector3(x + slideOrigin, y, 0.0f);
			if (Block.SlideFront)
			{
				center -= new Vector3(0, 0, -0.01f);
			}
			else
			{
				center -= new Vector3(0, 0, 0.01f);
			}
			
			Vector3 fromRelCenter = new Vector3(x, y, 0.0f) - center;
			Vector3 toRelCenter = new Vector3(x + slideOrigin * 2.0f, y, 0.0f) - center;
			float time = slider.SlideElapsed / BlockSlider.SlideDuration;
			Block.transform.position = Vector3.Slerp(fromRelCenter, toRelCenter, time);
			Block.transform.position += center;
			break;
			
		case Block.BlockState.Falling:
			y += gridElementLength - gridElementLength * Block.FallElapsed / Block.FallDuration;
			
			Block.transform.position = new Vector3(x, y, 0.0f);
			break;
			
		case Block.BlockState.Dying:
			// when dying, first we flash
			if (Block.DieElapsed < dyingFlashDuration)
			{
				float flash = Block.DieElapsed * 4.0f / dyingFlashDuration;
				if (flash > 2.0f)
					flash = 4.0f - flash;
				if (flash > 1.0f)
					flash = 2.0f - flash;
				
				Block.transform.Find("Cube").renderer.material.color = new Color(
					colors[Block.Type].r + flash * (flashColor.r - colors[Block.Type].r),
					colors[Block.Type].g + flash * (flashColor.g - colors[Block.Type].g),
					colors[Block.Type].b + flash * (flashColor.b - colors[Block.Type].b));
			}
			else
			{
				Block.transform.Find("Cube").renderer.material.color = colors[Block.Type];
				
				Block.transform.Find("Cube").transform.Rotate(new Vector3(Block.DyingAxis.x, Block.DyingAxis.y, 0.0f), Block.DieElapsed * Block.DieElapsed * Time.deltaTime * dyingSpeed);
				
                    float scale = 1.0f - Block.DieElapsed / Block.DieDuration;
                    
                    Block.transform.localScale = new Vector3(scale, scale, scale);
                }
                break;
        }
    }
}
