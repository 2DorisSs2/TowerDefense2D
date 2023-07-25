using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerActionSell : TowerAction
{
	
	public GameObject emptyPlacePrefab;

	
	void Awake()
	{
		Debug.Assert(emptyPlacePrefab, "Wrong initial parameters");
	}

	protected override void Clicked()
	{
		
		Tower tower = GetComponentInParent<Tower>();
		if (tower != null)
		{
			tower.SellTower(emptyPlacePrefab);
		}
	}
}
