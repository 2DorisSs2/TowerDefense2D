using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class TowersFolderInspector : MonoBehaviour
{
	
	public GameObject towerPrefab;
	
	public Transform towerFolder;

	
	void OnEnable()
	{
		Debug.Assert(towerPrefab && towerFolder, "Wrong stuff settings");
	}

	
	public GameObject AddTower()
	{
		int towerCount = FindObjectsOfType<Tower>().Length;
		GameObject tower = Instantiate(towerPrefab, towerFolder);
		tower.name = towerPrefab.name;
		if (towerCount > 0)
		{
			tower.name += " (" + towerCount.ToString() + ")";
		}
		tower.transform.SetAsLastSibling();
		return tower;
	}

	public GameObject GetNextBuildingPlace(GameObject currentSelected)
	{
		return InspectorsUtil<BuildingPlace>.GetNext(towerFolder, currentSelected);
	}

	
	public GameObject GetPrevioustBuildingPlace(GameObject currentSelected)
	{
		return InspectorsUtil<BuildingPlace>.GetPrevious(towerFolder, currentSelected);
	}
}
#endif
