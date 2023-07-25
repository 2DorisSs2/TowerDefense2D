using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeHeal : AiFeature
{
	
	public int healAmount = 1;
	
	public CircleCollider2D radius;
	
	public float cooldown = 3f;
	
	public GameObject healVisualPrefab;
	
	public float healVisualDuration = 1f;
	
	public List<string> tags = new List<string>();

	
	private float cooldownCounter;
	
	private Animator anim;


	void Start()
	{
		Debug.Assert(radius, "Wrong initial settings");
		anim = GetComponentInParent<Animator>();
		cooldownCounter = cooldown;
		radius.enabled = false;
	}

	void FixedUpdate()
	{
		if (cooldownCounter < cooldown)
		{
			cooldownCounter += Time.fixedDeltaTime;
		}
		else
		{
			cooldownCounter = 0f;
			
			if (Heal() == true)
			{
				if (anim != null && anim.runtimeAnimatorController != null)
				{
					
					foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
					{
						if (clip.name == "Special")
						{
							anim.SetTrigger("special");
							break;
						}
					}
				}
			}
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

	
	private bool Heal()
	{
		bool res = false;
		
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius.radius * transform.localScale.x);
		foreach (Collider2D col in cols)
		{
			if (IsTagAllowed(col.tag) == true)
			{
				
				DamageTaker target = col.gameObject.GetComponent<DamageTaker>();
				if (target != null)
				{
					
					if (target.currentHitpoints < target.hitpoints)
					{
						res = true;
						target.TakeDamage(-healAmount);
						if (healVisualPrefab != null)
						{
							
							GameObject effect = Instantiate(healVisualPrefab, target.transform);
							
							Destroy(effect, healVisualDuration);
						}
					}
				}
			}
		}
		return res;
	}
}
