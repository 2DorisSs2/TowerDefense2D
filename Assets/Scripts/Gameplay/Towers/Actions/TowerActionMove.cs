using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerActionMove : TowerAction
{
	
	private DefendPoint defendPoint;
	
	private Tower tower;


	void Start()
	{
		BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
		defendPoint = buildingPlace.GetComponentInChildren<DefendPoint>();
		tower = GetComponentInParent<Tower>();
		Debug.Assert(defendPoint && tower, "Wrong initial settings");
	}

	
	protected override void Clicked()
	{
		defendPoint.SetVisible(true);
		tower.ShowRange(true);
	}
}
