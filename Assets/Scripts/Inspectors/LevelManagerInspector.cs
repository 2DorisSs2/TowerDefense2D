using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class LevelManagerInspector : MonoBehaviour
{
	[HideInInspector]
	
	public List<GameObject> enemiesList = new List<GameObject>();

	[HideInInspector]
	
	public List<GameObject> enemies
	{
		get
		{
			return levelManager.allowedEnemies;
		}
		set
		{
			levelManager.allowedEnemies = value;
		}
	}


	
	public List<GameObject> towersList = new List<GameObject>();


	
	public List<GameObject> towers
	{
		get
		{
			return levelManager.allowedTowers;
		}
		set
		{
			levelManager.allowedTowers = value;
		}
	}


	public List<GameObject> spellsList = new List<GameObject>();


	
	public List<GameObject> spells
	{
		get
		{
			return levelManager.allowedSpells;
		}
		set
		{
			levelManager.allowedSpells = value;
		}
	}

	
	public int goldAmount
	{
		get
		{
			return levelManager.goldAmount;
		}
		set
		{
			levelManager.goldAmount = value;
		}
	}

	public int defeatAttempts
	{
		get
		{
			return levelManager.defeatAttempts;
		}
		set
		{
			levelManager.defeatAttempts = value;
		}
	}

	
	private LevelManager levelManager;


	void OnEnable()
	{
		levelManager = GetComponent<LevelManager>();
		Debug.Assert(levelManager, "Wrong stuff settings");
	}
}
#endif
