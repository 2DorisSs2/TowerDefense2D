using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefendersSpawner : MonoBehaviour
{
   
    public float cooldown = 10f;
   
    public int maxNum = 2;
  
    public GameObject prefab;
 
    public Transform spawnPoint;
	
	public DefendPoint defPoint;


    private float cooldownCounter;

	private Animator anim;



	void Start()
	{
		anim = GetComponent<Animator>();
		Debug.Assert(spawnPoint, "Wrong initial settings");
		BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
		defPoint = buildingPlace.GetComponentInChildren<DefendPoint>();
		cooldownCounter = cooldown;
	
		Dictionary<GameObject, Transform> oldDefenders = new Dictionary<GameObject, Transform>();
		foreach (KeyValuePair<GameObject, Transform> pair in defPoint.activeDefenders)
		{
			oldDefenders.Add(pair.Key, pair.Value);
		}
		defPoint.activeDefenders.Clear();
		foreach (KeyValuePair<GameObject, Transform> pair in oldDefenders)
		{
		
			Spawn(pair.Key.transform, pair.Value);
		}
	
		foreach (KeyValuePair<GameObject, Transform> pair in oldDefenders)
		{
			Destroy(pair.Key);
		}
	}


    void FixedUpdate()
    {
		cooldownCounter += Time.fixedDeltaTime;
        if (cooldownCounter >= cooldown)
        {
            
            if (TryToSpawn() == true)
            {
                cooldownCounter = 0f;
            }
            else
            {
                cooldownCounter = cooldown;
            }
        }
    }

   
    private bool TryToSpawn()
    {
        bool res = false;
        
		if ((prefab != null) && (defPoint.activeDefenders.Count < maxNum))
        {
			Transform destination = defPoint.GetFreeDefendPosition();
            
            if (destination != null)
            {
				
				Spawn(spawnPoint, destination);
                res = true;
            }
        }
        return res;
    }

	
	private void Spawn(Transform position, Transform destination)
	{
		
		GameObject obj = Instantiate<GameObject>(prefab, position.position, position.rotation);
		obj.name = prefab.name;
		
		obj.GetComponent<AiStateMove>().destination = destination;
		
		defPoint.activeDefenders.Add(obj, destination);
		
		if (anim != null && anim.runtimeAnimatorController != null)
		{
			foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
			{
				if (clip.name == "Spawn")
				{
					anim.SetTrigger("spawn");
					break;
				}
			}
		}
	}
}
