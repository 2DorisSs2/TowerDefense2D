using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WavesInfo : MonoBehaviour
{

	public List<float> wavesTimeouts = new List<float>();


	#if UNITY_EDITOR


	private float defaultWaveTimeout = 30f;

	private SpawnPoint[] spawners;

	
	public void Update()
	{
		spawners = FindObjectsOfType<SpawnPoint>();
		int wavesCount = 0;
	
		foreach (SpawnPoint spawner in spawners)
		{
			if (spawner.waves.Count > wavesCount)
			{
				wavesCount = spawner.waves.Count;
			}
		}
		
		if (wavesTimeouts.Count < wavesCount)
		{
			int i;
			for (i = wavesTimeouts.Count; i < wavesCount; ++i)
			{
				wavesTimeouts.Add (defaultWaveTimeout);
			}
		}
		else if (wavesTimeouts.Count > wavesCount)
		{
			wavesTimeouts.RemoveRange (wavesCount, wavesTimeouts.Count - wavesCount);
		}
	}

	#endif
}
