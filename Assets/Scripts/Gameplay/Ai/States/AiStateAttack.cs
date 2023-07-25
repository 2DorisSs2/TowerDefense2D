using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateAttack : AiState
{
	[Space(10)]
 
    public bool useTargetPriority = false;
   
	public AiState passiveAiState;


    private GameObject target;
   
    private List<GameObject> targetsList = new List<GameObject>();

    private Attack meleeAttack;
   
    private Attack rangedAttack;
 
    private Attack myLastAttack;
   
    private bool targetless;

    
    public override void Awake()
    {
		base.Awake();
        meleeAttack = GetComponentInChildren<AttackMelee>() as Attack;
        rangedAttack = GetComponentInChildren<AttackRanged>() as Attack;
    }

	
	public override void OnStateEnter(AiState previousState, AiState newState)
	{
		
		if (aiBehavior.navAgent != null)
		{
			aiBehavior.navAgent.move = false;
		}
	}

 
	public override void OnStateExit(AiState previousState, AiState newState)
    {
        LoseTarget();
    }

   
    void FixedUpdate()
    {
        
        if ((target == null) && (targetsList.Count > 0))
        {
            target = GetTopmostTarget();
        }
       
        if (target == null)
        {
            if (targetless == false)
            {
                targetless = true;
            }
            else
            {
               
                aiBehavior.ChangeState(passiveAiState);
            }
        }
    }

    
    private GameObject GetTopmostTarget()
    {
        GameObject res = null;
        if (useTargetPriority == true) 
        {
            float minPathDistance = float.MaxValue;
            foreach (GameObject ai in targetsList)
            {
                if (ai != null)
                {
                    AiStatePatrol aiStatePatrol = ai.GetComponent<AiStatePatrol>();
                    float distance = aiStatePatrol.GetRemainingPath();
                    if (distance < minPathDistance)
                    {
                        minPathDistance = distance;
                        res = ai;
                    }
                }
            }
        }
        else 
        {
            res = targetsList[0];
        }
       
        targetsList.Clear();
        return res;
    }

   
    private void LoseTarget()
    {
        target = null;
        targetless = false;
        myLastAttack = null;
    }

	
	public override bool OnTrigger(AiState.Trigger trigger, Collider2D my, Collider2D other)
	{
		if (base.OnTrigger(trigger, my, other) == false)
		{
			switch (trigger)
			{
			case AiState.Trigger.TriggerStay:
				TriggerStay(my, other);
				break;
			case AiState.Trigger.TriggerExit:
				TriggerExit(my, other);
				break;
			}
		}
		return false;
	}

    
	private void TriggerStay(Collider2D my, Collider2D other)
    {
        if (target == null) 
        {
            targetsList.Add(other.gameObject);
        }
        else
        {
            
            if (target == other.gameObject)
            {
                if (my.name == "MeleeAttack") 
                {
                    
                    if (meleeAttack != null)
                    {
						if (aiBehavior.navAgent != null)
						{
							
							aiBehavior.navAgent.LookAt(target.transform);
						}
                        
                        myLastAttack = meleeAttack as Attack;
                        
                        meleeAttack.TryAttack(other.transform);
                    }
                }
                else if (my.name == "RangedAttack") 
                {
                    
                    if (rangedAttack != null)
                    {
                       
                        if ((meleeAttack == null)
                            || ((meleeAttack != null) && (myLastAttack != meleeAttack)))
                        {
							if (aiBehavior.navAgent != null)
							{
								
								aiBehavior.navAgent.LookAt(target.transform);
							}
                           
                            myLastAttack = rangedAttack as Attack;
                            
                            rangedAttack.TryAttack(other.transform);
                        }
                    }
                }
            }
        }
    }


	private void TriggerExit(Collider2D my, Collider2D other)
    {
        if (other.gameObject == target)
        {
            
            LoseTarget();
        }
    }
}
