using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class SpawnPointInspector : MonoBehaviour
{


	public List<int> enemies = new List<int>();


	private SpawnPoint spawnPoint;


	void OnEnable()
	{
		spawnPoint = GetComponent<SpawnPoint>();
		Debug.Assert(spawnPoint, "Wrong stuff settings");
		
		enemies.Clear();
		foreach (SpawnPoint.Wave wave in spawnPoint.waves)
		{
			enemies.Add(wave.enemies.Count);
		}
	}

	
	public void UpdateWaveList()
	{
		
		while (spawnPoint.waves.Count > enemies.Count)
		{
			spawnPoint.waves.RemoveAt(spawnPoint.waves.Count - 1);
		}
		while (spawnPoint.waves.Count < enemies.Count)
		{
			spawnPoint.waves.Add(new SpawnPoint.Wave());
		}
		
		for (int i = 0; i < enemies.Count; i++)
		{
			while (spawnPoint.waves[i].enemies.Count > enemies[i])
			{
				spawnPoint.waves[i].enemies.RemoveAt(spawnPoint.waves[i].enemies.Count - 1);
			}
			while (spawnPoint.waves[i].enemies.Count < enemies[i])
			{
				spawnPoint.waves[i].enemies.Add(null);
			}
		}
	}

	
	public void AddWave()
	{
		enemies.Add(1);
	}

	
	public void RemoveWave()
	{
		if (enemies.Count > 0)
		{
			enemies.RemoveAt(enemies.Count - 1);
		}
	}
}
#endif
