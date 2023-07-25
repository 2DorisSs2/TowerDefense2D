using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]


public class MapFolderInspector : MonoBehaviour
{

	public SpriteRenderer map;

	public Transform spawnIconFolder;

	public Transform captureIconFolder;


	void OnEnable()
	{
		Debug.Assert(map && spawnIconFolder && captureIconFolder, "Wrong stuff settings");
	}


	public void ChangeMapSprite(Sprite sprite)
	{
		if (map != null && sprite != null)
		{
			map.sprite = sprite;
		}
	}

	public void LoadMap(GameObject mapPrefab)
	{
		if (mapPrefab != null)
		{
			if (map != null)
			{
				DestroyImmediate(map.gameObject);
			}
			GameObject newMap = Instantiate(mapPrefab, transform);
			newMap.name = mapPrefab.name;
			map = newMap.GetComponent<SpriteRenderer>();
			Debug.Assert(map, "Wrong stuff settings");
		}
	}


	public GameObject AddSpawnIcon(GameObject spawnIconPrefab)
	{
		GameObject res = Instantiate(spawnIconPrefab, spawnIconFolder);
		res.name = spawnIconPrefab.name;
		return res;
	}


	public GameObject AddCaptureIcon(GameObject captureIconPrefab)
	{
		GameObject res = Instantiate(captureIconPrefab, captureIconFolder);
		res.name = captureIconPrefab.name;
		return res;
	}
}
#endif
