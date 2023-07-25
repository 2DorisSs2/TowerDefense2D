using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloudOnDamage : MonoBehaviour
{
	
	public float duration = 3f;
	
	public float cooldown = 5f;
	
	public float radius = 1f;
	
	public Clouded cloudPrefab;
	
	public GameObject exhaustFX;
	
	public List<string> tags = new List<string>();

	
	private enum MyState
	{
		WaitForDamage,
		Clouded,
		Cooldown
	}
	
	private MyState myState = MyState.WaitForDamage;
	
	private float counter;

	
	void Start()
	{
		Debug.Assert(cloudPrefab && exhaustFX, "Wrong initial settings");
		counter = 0f;
	}
	
	
	void Update()
	{
		switch (myState)
		{
		case MyState.Cooldown:	
			if (counter > 0f)
			{
				counter -= Time.deltaTime;
			}
			else
			{
				counter = 0f;
				myState = MyState.WaitForDamage;
			}
			break;
		case MyState.Clouded:	
			if (counter > 0f)
			{
				counter -= Time.deltaTime;
			}
			else
			{
				counter = cooldown;
				myState = MyState.Cooldown;
			}
			break;
		}
	}

	
	public void OnDamage()
	{
		
		if (myState == MyState.WaitForDamage)
		{
			myState = MyState.Clouded;
			counter = duration;
			CloudNow();
		
			GameObject obj = Instantiate(exhaustFX, transform);
			Destroy(obj, duration);
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

	
	private void CloudNow()
	{
		
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
		foreach (Collider2D col in cols)
		{
			if (IsTagAllowed(col.tag) == true)
			{
		
				Clouded clouded = Instantiate(cloudPrefab, col.gameObject.transform);
				clouded.duration = duration;
			}
		}
	}
}
