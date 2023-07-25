using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiStateIdle : AiState
{

	public override void OnStateEnter(AiState previousState, AiState newState)
    {

		if (aiBehavior.navAgent != null)
		{
			aiBehavior.navAgent.move = false;
			aiBehavior.navAgent.turn = false;
		}

		if (anim != null && anim.runtimeAnimatorController != null)
		{

			foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
			{
				if (clip.name == "Idle")
				{
	
					anim.SetTrigger("idle");
					break;
				}
			}
		}
    }
}
