using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SpawnPoint : MonoBehaviour
{

	[System.Serializable]
	public class Wave
	{

		public float delayBeforeWave;
	
		public List<GameObject> enemies = new List<GameObject>();
	}

	
	public float speedRandomizer = 0.2f;
	
	public float unitSpawnDelay = 1.5f;

	public List<Wave> waves;

	public bool endlessWave = false;
	
	
	public List<GameObject> randomEnemiesList = new List<GameObject>();

	
	private Pathway path;
	
	private float counter;
	
	private List<GameObject> activeEnemies = new List<GameObject>();
	
	private bool finished = false;

	
	void Awake ()
	{
		path = GetComponentInParent<Pathway>();
		Debug.Assert(path != null, "Wrong initial parameters");
	}

	void OnEnable()
	{
		EventManager.StartListening("UnitDie", UnitDie);
		EventManager.StartListening("WaveStart", WaveStart);
	}

	
	void OnDisable()
	{
		EventManager.StopListening("UnitDie", UnitDie);
		EventManager.StopListening("WaveStart", WaveStart);
	}

	void Update()
	{
		
		if ((finished == true) && (activeEnemies.Count <= 0))
		{
			EventManager.TriggerEvent("AllEnemiesAreDead", null, null);
			gameObject.SetActive(false);
		}
	}

	
	private IEnumerator RunWave(int waveIdx)
	{
		if (waves.Count > waveIdx)
		{
			yield return new WaitForSeconds(waves[waveIdx].delayBeforeWave);

			while (endlessWave == true)
			{
				GameObject prefab = randomEnemiesList[Random.Range (0, randomEnemiesList.Count)];
				
				GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
				newEnemy.name = prefab.name;
				
				newEnemy.GetComponent<AiStatePatrol>().path = path;
				NavAgent agent = newEnemy.GetComponent<NavAgent>();
				
				agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
				
				activeEnemies.Add(newEnemy);
				
				yield return new WaitForSeconds(unitSpawnDelay);
			}

			foreach (GameObject enemy in waves[waveIdx].enemies)
			{
				GameObject prefab = null;
				prefab = enemy;
				
				if (prefab == null && randomEnemiesList.Count > 0)
				{
					prefab = randomEnemiesList[Random.Range (0, randomEnemiesList.Count)];
				}
				if (prefab == null)
				{
					Debug.LogError("Have no enemy prefab. Please specify enemies in Level Manager or in Spawn Point");
				}
				
				GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
				newEnemy.name = prefab.name;
				
				newEnemy.GetComponent<AiStatePatrol>().path = path;
				NavAgent agent = newEnemy.GetComponent<NavAgent>();
			
				agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
				
				activeEnemies.Add(newEnemy);
				
				yield return new WaitForSeconds(unitSpawnDelay);
			}
			if (waveIdx + 1 == waves.Count)
			{
				finished = true;
			}
		}
	}

	
	private void UnitDie(GameObject obj, string param)
	{
		
		if (activeEnemies.Contains(obj) == true)
		{
			
			activeEnemies.Remove(obj);
		}
	}

	
	private void WaveStart(GameObject obj, string param)
	{
		int waveIdx;
		int.TryParse(param, out waveIdx);
		StartCoroutine("RunWave", waveIdx);
	}

	
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
