using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#if UNITY_EDITOR
[ExecuteInEditMode]

public class BuildingPlaceInspector : MonoBehaviour
{

	private GameObject myTower;

	private GameObject defendPoint;


	void OnEnable()
	{
		defendPoint = GetComponentInChildren<DefendPoint>().gameObject;
		myTower = GetComponentInChildren<Tower>().gameObject;
		Debug.Assert(myTower && defendPoint, "Wrong stuff settings");
	}


	public GameObject GetDefendPoint()
	{
		return defendPoint;
	}



	public GameObject ChooseTower(GameObject towerPrefab)
	{

		if (myTower != null)
		{
			DestroyImmediate(myTower);
		}

		myTower = Instantiate(towerPrefab, transform);
		myTower.name = towerPrefab.name;
		myTower.transform.SetAsLastSibling();
		return myTower;
	}
}
//#endif
