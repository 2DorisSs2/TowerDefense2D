using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class LevelChooseInspector : MonoBehaviour
{

	public Transform levelFolder;

	
	private LevelChoose levelChooser;


	void OnEnable()
	{
		levelChooser = GetComponent<LevelChoose>();
		Debug.Assert(levelFolder && levelChooser, "Wrong initial settings");
		
		levelChooser.levelsPrefabs.RemoveAll(GameObject => GameObject == null);
	}

	public void AddLevel(GameObject levelPrefab)
	{
		if (levelPrefab != null)
		{
			
			if (levelChooser.levelsPrefabs.Contains(levelPrefab) == false)
			{
				levelChooser.levelsPrefabs.Add(levelPrefab);
			}
		}
	}

	
	public void SetActiveLevel(GameObject level)
	{
		LevelDescriptionInspector oldLevel = levelFolder.GetComponentInChildren<LevelDescriptionInspector>();
	
		if (oldLevel != null)
		{
			DestroyImmediate(oldLevel.gameObject);
		}
	
		level.transform.SetParent(levelFolder, false);
		level.transform.SetAsFirstSibling();
		levelChooser.currentLevel = level;
	}

	public List<GameObject> GetLevelPrefabs()
	{
		return levelChooser.levelsPrefabs;
	}
}
#endif
