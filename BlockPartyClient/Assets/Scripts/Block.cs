using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	public int X, Y;
	public int Type;
	public const int TypeCount = 5;

	// Use this for initialization
	void Start ()
	{
	
	}

	public void Initialize(int x, int y, int type)
	{
		X = x;
		Y = y;
		Type = type;

		transform.position = new Vector3(X, Y, 0.0f);
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
