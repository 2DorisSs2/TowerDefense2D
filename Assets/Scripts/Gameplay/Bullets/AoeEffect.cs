using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeEffect : MonoBehaviour
{

	public string effectName;

	public float modifier = 1f;

	public float duration = 3f;

	public float radius = 1f;

	public GameObject explosionFx;
	
	public float explosionFxDuration = 1f;
	
	public GameObject effectFx;
	
	public AudioClip sfx;
	
	public List<string> tags = new List<string>();

	
	private bool isQuitting;

	
	void OnEnable()
	{
		EventManager.StartListening("SceneQuit", SceneQuit);
	}

	void OnDisable()
	{
		EventManager.StopListening("SceneQuit", SceneQuit);
	}

	
	void OnApplicationQuit()
	{
		isQuitting = true;
	}

	
	void OnDestroy()
	{
		
		if (isQuitting == false)
		{
			
			Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
			foreach (Collider2D col in cols)
			{
				if (IsTagAllowed(col.tag) == true)
				{
					EffectControl effectControl = col.gameObject.GetComponent<EffectControl>();
					if (effectControl != null)
					{
						effectControl.AddEffect(effectName, modifier, duration, effectFx);
					}
				}
			}
			if (explosionFx != null)
			{
				
				Destroy(Instantiate<GameObject>(explosionFx, transform.position, transform.rotation), explosionFxDuration);
			}
			if (sfx != null && AudioManager.instance != null)
			{
				
				AudioManager.instance.PlaySound(sfx);
			}
		}
	}

	
	private void SceneQuit(GameObject obj, string param)
	{
		isQuitting = true;
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
