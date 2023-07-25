using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Heal : AiFeature
{
	
	public int healAmount = 1;

	public float cooldown = 3f;

	public GameObject healVisualPrefab;

	public float healVisualDuration = 1f;

	public List<string> tags = new List<string>();


	private float cooldownCounter;

	
	void Start()
	{
		cooldownCounter = cooldown;
	}

	
	void FixedUpdate()
	{
		if (cooldownCounter < cooldown)
		{
			cooldownCounter += Time.fixedDeltaTime;
		}
	}

	
	private bool IsTagAllowed(string tag)
	{
		bool res = false;
		if (tags.Count > 0)
		{
			foreach (string str in tags)
			{
				if (str == tag)
				{
					res = true;
					break;
				}
			}
		}
		else
		{
			res = true;
		}
		return res;
	}

	
	private void TryToHeal(DamageTaker target)
	{
		
		if (cooldownCounter >= cooldown)
		{
			cooldownCounter = 0f;
			target.TakeDamage(-healAmount);
			if (healVisualPrefab != null)
			{
				GameObject effect = Instantiate(healVisualPrefab, target.transform);
				
				Destroy(effect, healVisualDuration);
			}
		}
	}


	void OnTriggerStay2D(Collider2D other)
	{
		if (IsTagAllowed(other.tag) == true)
		{
		
			DamageTaker target = other.gameObject.GetComponent<DamageTaker>();
			if (target != null)
			{
				
				if (target.currentHitpoints < target.hitpoints)
				{
					TryToHeal(target);
				}
			}
		}
	}
}
