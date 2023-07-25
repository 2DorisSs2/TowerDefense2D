using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class DataManagerInspector : MonoBehaviour
{
	
	private DataManager dataManager;

	
	void OnEnable()
	{
		dataManager = GetComponent<DataManager>();
		Debug.Assert(dataManager, "Wrong initial settings");
	}

	
	public void ResetGameProgress()
	{
		dataManager.DeleteGameProgress();
	}

	
	public void PermitLevel(string levelName)
	{
		if (dataManager.progress.openedLevels.Contains(levelName) == false)
		{
			dataManager.progress.openedLevels.Add(levelName);
			dataManager.SaveGameProgress();
		}
	}
}
#endif
