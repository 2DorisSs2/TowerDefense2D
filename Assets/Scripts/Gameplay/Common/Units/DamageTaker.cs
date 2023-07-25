using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageTaker : MonoBehaviour
{
 
    public int hitpoints = 1;


    public int currentHitpoints;

    public float damageDisplayTime = 0.2f;

    public Transform healthBar;

	public bool isTrigger;

	public AudioClip dieSfx;


    private SpriteRenderer sprite;

	private bool coroutineInProgress;

    private float originHealthBarWidth;

  
    void Awake()
    {
        currentHitpoints = hitpoints;
        sprite = GetComponentInChildren<SpriteRenderer>();
        Debug.Assert(sprite && healthBar, "Wrong initial parameters");
    }


    void Start()
    {
        originHealthBarWidth = healthBar.localScale.x;
    }



    public void TakeDamage(int damage)
    {
		if (damage > 0)
		{
			if (this.enabled == true)
			{
				if (currentHitpoints > damage)
				{
			
					currentHitpoints -= damage;
					UpdateHealthBar();
			
					if (coroutineInProgress == false)
					{
						
						StartCoroutine(DisplayDamage());
					}
					if (isTrigger == true)
					{
						
						SendMessage("OnDamage");
					}
				}
				else
				{
			
					currentHitpoints = 0;
					UpdateHealthBar();
					Die();
				}
			}
		}
		else 
		{
		
			currentHitpoints = Mathf.Min(currentHitpoints - damage, hitpoints);
			UpdateHealthBar();
		}
    }


    private void UpdateHealthBar()
    {
        float healthBarWidth = originHealthBarWidth * currentHitpoints / hitpoints;
        healthBar.localScale = new Vector2(healthBarWidth, healthBar.localScale.y);
    }

 
    public void Die()
    {
		EventManager.TriggerEvent("UnitKilled", gameObject, null);
		StartCoroutine(DieCoroutine());
    }

	private IEnumerator DieCoroutine()
	{
		if (dieSfx != null && AudioManager.instance != null)
		{
			AudioManager.instance.PlayDie(dieSfx);
		}
		foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
		{
			col.enabled = false;
		}
		GetComponent<AiBehavior>().enabled = false;
		GetComponent<NavAgent>().enabled = false;
		GetComponent<EffectControl>().enabled = false;
		Animator anim = GetComponent<Animator>();
		
		if (anim != null && anim.runtimeAnimatorController != null)
		{
			
			foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
			{
				if (clip.name == "Die")
				{
					
					anim.SetTrigger("die");
					yield return new WaitForSeconds(clip.length);
					break;
				}
			}
		}
		Destroy(gameObject);
	}

    
    IEnumerator DisplayDamage()
    {
        coroutineInProgress = true;
        Color originColor = sprite.color;
        float counter;
        
		for (counter = 0f; counter < damageDisplayTime; counter += Time.fixedDeltaTime)
        {
            sprite.color = Color.Lerp(originColor, Color.black, Mathf.PingPong(counter, damageDisplayTime / 2f));
			yield return new WaitForFixedUpdate();
        }
        sprite.color = originColor;
        coroutineInProgress = false;
    }

	void OnDestroy()
	{
		EventManager.TriggerEvent("UnitDie", gameObject, null);
		StopAllCoroutines();
	}
}
