using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTriggerAlone : MonoBehaviour
{

	public float aloneDuration = 1f;
	public List<Component> receivers = new List<Component>();

	public List<string> tags = new List<string>();


	private AiBehavior ai;

	private Collider2D col;

	private float counter;

	private bool triggered;

	void Awake()
	{
		ai = GetComponentInParent<AiBehavior>();
		col = GetComponent<Collider2D>();
		Debug.Assert(ai && col, "Wrong initial parameters");
		col.enabled = false;
	}

	void Start()
	{
		col.enabled = true;
		counter = aloneDuration;
		triggered = false;
	}

	
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject != ai.gameObject && IsTagAllowed(other.tag) == true)
		{
			if (triggered == true)
			{
				foreach (Component receiver in receivers)
				{
					receiver.SendMessage("OnTriggerAloneEnd");
				}
			}
			triggered = false;
			counter = aloneDuration;
		}
	}

	void FixedUpdate()
	{
		if (triggered == false)
		{
			if (counter > 0f)
			{
				counter -= Time.fixedDeltaTime;
			}
			else
			{
				triggered = true;
				counter = 0f;
				foreach (Component receiver in receivers)
				{
					receiver.SendMessage("OnTriggerAloneStart");
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
}
