using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealEffect : TowerSkillEffect
{
	
	public GameObject healFxPrefab;

	public float fxDuration = 1f;

	
	void Start()
	{
		DefendersSpawner defendersSpawner = tower.GetComponent<DefendersSpawner>();
		if (defendersSpawner != null)
		{
			
			foreach (GameObject defender in defendersSpawner.defPoint.GetDefenderList())
			{
				DamageTaker damageTaker = defender.GetComponent<DamageTaker>();
				
				damageTaker.TakeDamage(-damageTaker.hitpoints);
				if (healFxPrefab != null)
				{
					
					Destroy(Instantiate(healFxPrefab, defender.transform), fxDuration);
				}
			}
		}
		else
		{
			Debug.Log("This tower can not use heal skills");
		}
		Destroy(gameObject);
	}
}
