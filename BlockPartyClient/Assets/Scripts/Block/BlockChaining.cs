using UnityEngine;
using System.Collections;

public class BlockChaining : MonoBehaviour {
	public Chain Chain;

	// Use this for initialization
	void Start () {
	
	}

	public void BeginChainInvolvement(Chain chain)
	{
		if (Chain != null)
		{
			Chain.DecrementInvolvement();
		}
		
		Chain = chain;
		Chain.IncrementInvolvement();
	}
	
	public void EndChainInvolvement(Chain chain)
	{
		if (Chain != null && Chain == chain)
		{
			Chain.DecrementInvolvement();
			Chain = null;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
