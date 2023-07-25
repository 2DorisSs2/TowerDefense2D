using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class PathwaysFolderInspector : MonoBehaviour
{

	public GameObject pathwayPrefab;

	public Transform pathwayFolder;

	public GameObject capturePointPrefab;

	public Transform capturePointFolder;


	void OnEnable()
	{
		Debug.Assert(pathwayPrefab && pathwayFolder && capturePointPrefab && capturePointFolder, "Wrong stuff settings");
	}


	public GameObject AddPathway()
	{
		Pathway[] array = GetComponentsInChildren<Pathway>();
		GameObject res = Instantiate(pathwayPrefab, pathwayFolder).gameObject;
		res.transform.SetAsLastSibling();
		res.name = pathwayPrefab.name + " (" + (array.Length + 1) + ")";
		return res;
	}


	public GameObject GetNextPathway(GameObject currentSelected)
	{
		return InspectorsUtil<Pathway>.GetNext(pathwayFolder, currentSelected);
	}


	public GameObject GetPrevioustPathway(GameObject currentSelected)
	{
		return InspectorsUtil<Pathway>.GetPrevious(pathwayFolder, currentSelected);
	}


	public GameObject AddCapturePoint()
	{
		CapturePoint[] array = GetComponentsInChildren<CapturePoint>();
		GameObject res = Instantiate(capturePointPrefab, capturePointFolder).gameObject;
		res.transform.SetSiblingIndex(array.Length);
		res.name = capturePointPrefab.name + " (" + (array.Length + 1) + ")";
		return res;
	}

	public GameObject GetNextCapturePoint(GameObject currentSelected)
	{
		return InspectorsUtil<CapturePoint>.GetNext(capturePointFolder, currentSelected);
	}


	public GameObject GetPrevioustCapturePoint(GameObject currentSelected)
	{
		return InspectorsUtil<CapturePoint>.GetPrevious(capturePointFolder, currentSelected);
	}
}
#endif
