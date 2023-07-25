using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChoose : MonoBehaviour
{
	
    public string exitSceneName;
	
	public Transform togglesFolder;
	
	public Toggle activeTogglePrefab;
	
	public Toggle inactiveTogglePrefab;
	
	public Button nextLevelButton;
	
	public Button prevLevelButton;
	
	public Transform levelFolder;
	
	public GameObject currentLevel;
	
	public List<GameObject> levelsPrefabs = new List<GameObject>();

	
	private int maxActiveLevelIdx;
	
	private int currentDisplayedLevelIdx;
	
	private List<Toggle> activeToggles = new List<Toggle>();

	
	void OnEnable()
	{
		EventManager.StartListening("ButtonPressed", ButtonPressed);
	}

	
	void OnDisable()
	{
		EventManager.StopListening("ButtonPressed", ButtonPressed);
	}

	
	void Awake()
	{
		maxActiveLevelIdx = -1;
		Debug.Assert(currentLevel && togglesFolder && activeTogglePrefab && inactiveTogglePrefab && nextLevelButton && prevLevelButton && levelFolder, "Wrong initial settings");
	}

	
    void Start()
    {
		int hitIdx = -1;
		int levelsCount = DataManager.instance.progress.openedLevels.Count;
		if (levelsCount > 0)
		{
			
			string openedLevelName = DataManager.instance.progress.openedLevels[levelsCount - 1];

	        int idx;
			for (idx = 0; idx < levelsPrefabs.Count; ++idx)
	        {
				
				if (levelsPrefabs[idx].name == openedLevelName)
	            {
	                hitIdx = idx;
	                break;
	            }
	        }
		}
		
		if (hitIdx >= 0)
		{
			if (levelsPrefabs.Count > hitIdx + 1)
			{
				maxActiveLevelIdx = hitIdx + 1;
			}
			else
			{
				maxActiveLevelIdx = hitIdx;
			}
		}
		
		else
		{
			if (levelsPrefabs.Count > 0)
			{
				maxActiveLevelIdx = 0;
			}
			else
			{
				Debug.LogError("Have no levels prefabs!");
			}
		}
		if (maxActiveLevelIdx >= 0)
		{
			DisplayToggles();
			DisplayLevel(maxActiveLevelIdx);
		}
    }

	
	private void DisplayToggles()
	{
		foreach (Toggle toggle in togglesFolder.GetComponentsInChildren<Toggle>())
		{
			Destroy(toggle.gameObject);
		}
		int cnt;
		for (cnt = 0; cnt < maxActiveLevelIdx + 1; cnt++)
		{
			GameObject toggle = Instantiate(activeTogglePrefab.gameObject, togglesFolder);
			activeToggles.Add(toggle.GetComponent<Toggle>());
		}
		if (maxActiveLevelIdx < levelsPrefabs.Count - 1)
		{
			Instantiate(inactiveTogglePrefab.gameObject, togglesFolder);
		}
	}

	
	private void DisplayLevel(int levelIdx)
	{
		Transform parentOfLevel = currentLevel.transform.parent;
		Vector3 levelPosition = currentLevel.transform.position;
		Quaternion levelRotation = currentLevel.transform.rotation;
		Destroy(currentLevel);
		currentLevel = Instantiate(levelsPrefabs[levelIdx], parentOfLevel);
		currentLevel.name = levelsPrefabs[levelIdx].name;
		currentLevel.transform.position = levelPosition;
		currentLevel.transform.rotation = levelRotation;
		currentDisplayedLevelIdx = levelIdx;
		foreach (Toggle toggle in activeToggles)
		{
			toggle.isOn = false;
		}
		activeToggles[levelIdx].isOn = true;
		UpdateButtonsVisible (levelIdx);
	}

	
	private void UpdateButtonsVisible(int levelIdx)
	{
		prevLevelButton.interactable = levelIdx > 0 ? true : false;
		nextLevelButton.interactable = levelIdx < maxActiveLevelIdx ? true : false;
	}

	
	private void DisplayNextLevel()
	{
		if (currentDisplayedLevelIdx < maxActiveLevelIdx)
		{
			DisplayLevel(currentDisplayedLevelIdx + 1);
		}
	}

	
	private void DisplayPrevLevel()
	{
		if (currentDisplayedLevelIdx > 0)
		{
			DisplayLevel (currentDisplayedLevelIdx - 1);
		}
	}

	
	private void Exit()
	{
		SceneManager.LoadScene(exitSceneName);
	}

	
	private void GoToLevel()
	{
		SceneManager.LoadScene(currentLevel.name);
	}

	
	private void ButtonPressed(GameObject obj, string param)
	{
		switch (param)
		{
		case "Start":
			GoToLevel();
			break;
		case "Exit":
			Exit();
			break;
		case "Next":
			DisplayNextLevel();
			break;
		case "Prev":
			DisplayPrevLevel();
			break;
		}
	}
}
