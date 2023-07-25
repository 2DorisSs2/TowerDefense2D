using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBold : MonoBehaviour, IBullet
{
   
	[HideInInspector] 
    public int damage = 1;
  
    public float lifeTime = 3f;
   
    public float speed = 3f;
  
    public float speedUpOverTime = 0.5f;
    
    public float hitDistance = 0.2f;
    
    public float ballisticOffset = 0.1f;
    
    public float penetrationRatio = 0.3f;
	
	public List<string> tags = new List<string>();

    
    private Vector2 originPoint;
  
    private Transform target;
  
    private Vector2 aimPoint;
   
    private Vector2 myVirtualPosition;
   
    private Vector2 myPreviousPosition;
   
    private float counter;
   
    private SpriteRenderer sprite;

  
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }


	public int GetDamage()
	{
		return damage;
	}

    
    public void Fire(Transform target)
    {
        sprite = GetComponent<SpriteRenderer>();
		Debug.Assert(sprite, "Wrong initial settings");
       
        sprite.enabled = false;
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        this.target = target;
        aimPoint = GetPenetrationPoint(target.position);
        
        Destroy(gameObject, lifeTime);
    }

   
    void FixedUpdate()
    {
		counter += Time.fixedDeltaTime;
        
		speed += Time.fixedDeltaTime * speedUpOverTime;
        if (target != null)
        {
            aimPoint = GetPenetrationPoint(target.position);
        }
       
        Vector2 originDistance = aimPoint - originPoint;
       
        Vector2 distanceToAim = aimPoint - (Vector2)myVirtualPosition;
        
        myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * speed / originDistance.magnitude);
        
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);
        
        LookAtDirection2D((Vector2)transform.position - myPreviousPosition);
        myPreviousPosition = transform.position;
        sprite.enabled = true;

        if (distanceToAim.magnitude <= hitDistance)
        {

            Destroy(gameObject);
        }
    }


    private void LookAtDirection2D(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private Vector2 GetPenetrationPoint(Vector2 targetPosition)
    {
        Vector2 penetrationVector = targetPosition - originPoint;
        return originPoint + penetrationVector * (1f + penetrationRatio);
    }



    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset > 0f)
        {
           
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            
            return (Vector2)myVirtualPosition + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            return myVirtualPosition;
        }
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
		if (IsTagAllowed(other.tag) == true)
		{
	     
	        DamageTaker damageTaker = other.GetComponent<DamageTaker> ();
	        if (damageTaker != null)
	        {
	            damageTaker.TakeDamage(damage);
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
