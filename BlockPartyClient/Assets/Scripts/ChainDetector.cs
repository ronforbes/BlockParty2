﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainDetector : MonoBehaviour {
	public Score Score;
	//public SignManager SignManager;
	List<Chain> chains = new List<Chain>(chainCapacity);
	const int chainCapacity = 8;
	
	public Chain CreateChain()
	{
		if (chains.Count == chains.Capacity)
			return null;
		
		Chain chain = new Chain(/*SignManager*/);
		chains.Add(chain);
		
		return chain;
	}
	
	public void DeleteChain(Chain chain)
	{
		chains.Remove(chain);
	}
	
	void Update()
	{
		List<Chain> chainsToRemove = new List<Chain>();
		
		foreach (Chain chain in chains)
		{
			if (chain.InvolvementCount == 0)
			{
				chainsToRemove.Add(chain);
			}
			else
			{
				if (chain.MatchJustOccurred)
				{
					// notify the score
					Score.ReportChain(chain);
					
					chain.MatchJustOccurred = false;
				}
			}
		}
		
		foreach (Chain chain in chainsToRemove)
		{
			DeleteChain(chain);
		}
	}
}
