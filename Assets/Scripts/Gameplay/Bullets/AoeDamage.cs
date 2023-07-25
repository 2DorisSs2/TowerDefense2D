using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AoeDamage : MonoBehaviour
{
	
	public float aoeDamageRate = 1f;

    public float radius = 0.3f;

    public GameObject explosion;

    public float explosionDuration = 1f;

	public AudioClip sfx;
	
	public List<string> tags = new List<string>();

	
	private IBullet bullet;
 
    private bool isQuitting;


	void Awake()
	{
		bullet = GetComponent<IBullet>();
		Debug.Assert(bullet != null, "Wrong initial settings");
	}

   
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
					
					DamageTaker damageTaker = col.gameObject.GetComponent<DamageTaker>();
					if (damageTaker != null)
					{
						
						damageTaker.TakeDamage((int)(Mathf.Ceil(aoeDamageRate * (float)bullet.GetDamage())));
					}
				}
            }
            if (explosion != null)
            {
                
                Destroy(Instantiate<GameObject>(explosion, transform.position, transform.rotation), explosionDuration);
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
