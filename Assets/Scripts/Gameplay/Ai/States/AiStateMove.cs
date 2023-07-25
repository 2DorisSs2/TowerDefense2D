using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateMove : AiState
{
	[Space(10)]


	public AiState passiveAiState;

	[HideInInspector]
	public Transform destination;

	public override void Awake()
	{
		base.Awake();
		Debug.Assert (aiBehavior.navAgent, "Wrong initial parameters");
	}

 
	public override void OnStateEnter(AiState previousState, AiState newState)
    {
   
		aiBehavior.navAgent.destination = destination.position;

		aiBehavior.navAgent.move = true;
		aiBehavior.navAgent.turn = true;

        if (anim != null && anim.runtimeAnimatorController != null)
        {
	
			foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
			{
				if (clip.name == "Move")
				{
		
					anim.SetTrigger("move");
					break;
				}
			}
        }
    }

   
	public override void OnStateExit(AiState previousState, AiState newState)
    {

		aiBehavior.navAgent.move = false;
		aiBehavior.navAgent.turn = false;
    }


    void FixedUpdate()
    {
        
        if ((Vector2)transform.position == (Vector2)destination.position)
        {
           
			aiBehavior.navAgent.LookAt(destination.right);
       
            aiBehavior.ChangeState(passiveAiState);
        }
    }
}
