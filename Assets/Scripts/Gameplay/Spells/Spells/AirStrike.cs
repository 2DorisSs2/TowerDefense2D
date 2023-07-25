using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AirStrike : MonoBehaviour
{
	
	public float[] delaysBeforeDamage = {0.5f};
	
	public int damage = 5;
	
	public float radius = 1f;
	
	public GameObject effectPrefab;
	
	public float effectDuration = 2f;

	public AudioClip sfx;


	private enum MyState
	{
		WaitForClick,
		WaitForFX
	}
	
	private MyState myState = MyState.WaitForClick;

	
	void OnEnable()
	{
		EventManager.StartListening("UserClick", UserClick);
		EventManager.StartListening("UserUiClick", UserUiClick);
	}

	
	void OnDisable()
	{
		EventManager.StopListening("UserClick", UserClick);
		EventManager.StopListening("UserUiClick", UserUiClick);
	}

	
	void Start()
	{
		Debug.Assert(effectPrefab, "Wrong initial settings");
	}

	
	private void UserClick(GameObject obj, string param)
	{
		if (myState == MyState.WaitForClick)
		{
			
			if (obj == null || obj.CompareTag("Tower") == false)
			{
				
				transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
				Destroy(effect, effectDuration);
				EventManager.TriggerEvent("ActionStart", gameObject, null);
			
				StartCoroutine(DamageCoroutine());
				myState = MyState.WaitForFX;
			}
			else 
			{
				Destroy(gameObject);
			}
		}
	}

	
	private void UserUiClick(GameObject obj, string param)
	{
	
		if (myState == MyState.WaitForClick)
		{
			Destroy(gameObject);
		}
	}

	
	private IEnumerator DamageCoroutine()
	{
		foreach (float delay in delaysBeforeDamage)
		{
			yield return new WaitForSeconds(delay);

		
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
			foreach (Collider2D col in hits)
			{
				if (col.CompareTag("Enemy") == true || col.CompareTag("FlyingEnemy") == true)
				{
					DamageTaker damageTaker = col.GetComponent<DamageTaker>();
					if (damageTaker != null)
					{
						damageTaker.TakeDamage(damage);
					}
				}
			}
			if (sfx != null && AudioManager.instance != null)
			{
			
				AudioManager.instance.PlaySound(sfx);
			}
		}

		Destroy(gameObject);
	}

	
	void OnDestroy()
	{
		if (myState == MyState.WaitForClick)
		{
			EventManager.TriggerEvent("ActionCancel", gameObject, null);
		}
		StopAllCoroutines();
	}
}
