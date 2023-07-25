using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiStatePatrol : AiState
{
	[Space(10)]
	[HideInInspector]
  
    public Pathway path;

    public bool loop = false;
	[HideInInspector]

	public Waypoint destination;


	public override void Awake()
    {
		base.Awake();
		Debug.Assert (aiBehavior.navAgent, "Wrong initial parameters");
    }


	public override void OnStateEnter(AiState previousState, AiState newState)
    {
        if (path == null)
        {
        
            path = FindObjectOfType<Pathway>();
            Debug.Assert(path, "Have no path");
        }
        if (destination == null)
        {
        
            destination = path.GetNearestWaypoint(transform.position);
        }
   
		aiBehavior.navAgent.destination = destination.transform.position;

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
        if (destination != null)
        {
   
            if ((Vector2)destination.transform.position == (Vector2)transform.position)
            {
     
                destination = path.GetNextWaypoint (destination, loop);
                if (destination != null)
                {
      
					aiBehavior.navAgent.destination = destination.transform.position;
                }
            }
        }
    }

    public float GetRemainingPath()
    {
        Vector2 distance = destination.transform.position - transform.position;
        return (distance.magnitude + path.GetPathDistance(destination));
    }


	public void UpdateDestination(bool getNearestWaypoint)
	{
		if (getNearestWaypoint == true)
		{
		
			destination = path.GetNearestWaypoint(transform.position);
		}
		if (enabled == true)
		{
		
			aiBehavior.navAgent.destination = destination.transform.position;
		}
	}
}
