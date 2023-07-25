using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouded : MonoBehaviour
{
	[HideInInspector]
	
	public float duration;

	
	private Collider2D col;
	
	private float counter;

	
	void Start()
	{
		col = GetComponentInParent<Collider2D>();
		Debug.Assert(col, "Wrong initial settings");
		counter = duration;
		
		col.enabled = false;
	}
	
	
	void FixedUpdate()
	{
		if (counter > 0f)
		{
			counter -= Time.fixedDeltaTime;
		}
		else
		{
			
			col.enabled = true;
			Destroy(gameObject);
		}
	}
}
