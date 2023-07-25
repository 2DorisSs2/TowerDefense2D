using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]


public class TowerActionsInspector : MonoBehaviour
{
	
	public float radialDistance = 1.4f;
	
	public float angleOffset = 180f;
	
	public List<TowerAction> actions = new List<TowerAction>();

	
	void Update()
	{
		
		actions.Clear();
		foreach (TowerAction action in GetComponentsInChildren<TowerAction>())
		{
			actions.Add(action);
		}
		PlaceAround();
	}

	
	private void PlaceAround()
	{
		float deltaAngle = 360f / actions.Count;
		for (int i = 0; i < actions.Count; ++i)
		{
			float radians = (angleOffset + i * deltaAngle) * Mathf.Deg2Rad;
			actions[i].transform.localPosition = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)) * radialDistance;
		}
	}

	public GameObject AddAction(GameObject actionPrefab)
	{
		GameObject res = Instantiate(actionPrefab, transform);
		res.name = actionPrefab.name;
		Update();
		return res;
	}

	public GameObject GetNextAction(GameObject currentSelected)
	{
		return InspectorsUtil<TowerAction>.GetNext(transform, currentSelected);
	}

	public GameObject GetPrevioustAction(GameObject currentSelected)
	{
		return InspectorsUtil<TowerAction>.GetPrevious(transform, currentSelected);
	}
}
#endif
