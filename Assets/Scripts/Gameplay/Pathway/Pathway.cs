using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Pathway : MonoBehaviour
{
	#if UNITY_EDITOR
    
    void Update()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        if (waypoints.Length > 1)
        {
            int idx;
            for (idx = 1; idx < waypoints.Length; idx++)
            {
               
				Debug.DrawLine(waypoints[idx - 1].transform.position, waypoints[idx].transform.position, new Color(0.7f, 0f, 0f));
            }
        }
    }
	#endif

  
    public Waypoint GetNearestWaypoint(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Waypoint nearestWaypoint = null;
        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            
            Vector3 vect = position - waypoint.transform.position;
            float distance = vect.magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWaypoint = waypoint;
            }
        }
		for (;;)
		{
			float waypointPathDistance = GetPathDistance(nearestWaypoint);
			Waypoint nextWaypoint = GetNextWaypoint(nearestWaypoint, false);
			if (nextWaypoint != null)
			{
				float myPathDistance = GetPathDistance(nextWaypoint) + (nextWaypoint.transform.position - position).magnitude;
				if (waypointPathDistance <= myPathDistance)
				{
					break;
				}
				else
				{
					nearestWaypoint = nextWaypoint;
				}
			}
			else
			{
				break;
			}
		}
        return nearestWaypoint;
    }

    
    public Waypoint GetNextWaypoint(Waypoint currentWaypoint, bool loop)
    {
        Waypoint res = null;
        int idx = currentWaypoint.transform.GetSiblingIndex();
        if (idx < (transform.childCount - 1))
        {
            idx += 1;
        }
        else
        {
            idx = 0;
        }
        if (!(loop == false && idx == 0))
        {
            res = transform.GetChild(idx).GetComponent<Waypoint>(); 
        }
        return res;
    }

	
	public Waypoint GetPreviousWaypoint(Waypoint currentWaypoint, bool loop)
	{
		Waypoint res = null;
		int idx = currentWaypoint.transform.GetSiblingIndex();
		if (idx > 0)
		{
			idx -= 1;
		}
		else
		{
			idx = transform.childCount - 1;
		}
		if (!(loop == false && idx == transform.childCount - 1))
		{
			res = transform.GetChild(idx).GetComponent<Waypoint>(); 
		}
		return res;
	}

	
    public float GetPathDistance(Waypoint fromWaypoint)
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        bool hitted = false;
        float pathDistance = 0f;
        int idx;
		
        for (idx = 0; idx < waypoints.Length; ++idx)
        {
            if (hitted == true)
            {
				
                Vector2 distance = waypoints[idx].transform.position - waypoints[idx - 1].transform.position;
                pathDistance += distance.magnitude;
            }
            if (waypoints[idx] == fromWaypoint)
            {
                hitted = true;
            }
        }
        return pathDistance;
    }

	
	public Vector2 GetOffsetPosition(ref Waypoint nextWaypoint, Vector2 currentPosition, float offsetDistance)
	{
		Vector2 res = currentPosition;
		if (offsetDistance >= 0f) 
		{
			float remainingDistance = offsetDistance;
			Vector2 lastPosition = currentPosition;
			Waypoint waypoint = nextWaypoint;
			Vector2 deltaVector = Vector2.zero;
			
			for (;;)
			{
				deltaVector = (Vector2)waypoint.transform.position - lastPosition;
				float deltaDistance = deltaVector.magnitude;
				if (remainingDistance > deltaDistance)
				{
					remainingDistance -= deltaDistance;
					lastPosition = waypoint.transform.position;
					waypoint = GetNextWaypoint(waypoint, false);
					if (waypoint == null)
					{
						remainingDistance = 0f;
						break;
					}
					else
					{
						nextWaypoint = waypoint;
					}
				}
				else
				{
					break;
				}
			}
			
			res = lastPosition + deltaVector.normalized * remainingDistance;
		}
		else 
		{
			float remainingDistance = -offsetDistance;
			Vector2 lastPosition = currentPosition;
			Waypoint waypoint = GetPreviousWaypoint(nextWaypoint, false);
			if (waypoint == null)
			{
				return res;
			}
			Vector2 deltaVector = Vector2.zero;
			
			for (;;)
			{
				deltaVector = (Vector2)waypoint.transform.position - lastPosition;
				float deltaDistance = deltaVector.magnitude;
				if (remainingDistance > deltaDistance)
				{
					nextWaypoint = waypoint;
					remainingDistance -= deltaDistance;
					lastPosition = waypoint.transform.position;
					waypoint = GetPreviousWaypoint(waypoint, false);
					if (waypoint == null)
					{
						remainingDistance = 0f;
						break;
					}
				}
				else
				{
					break;
				}
			}
			
			res = lastPosition + deltaVector.normalized * remainingDistance;
		}
		return res;
	}
}
