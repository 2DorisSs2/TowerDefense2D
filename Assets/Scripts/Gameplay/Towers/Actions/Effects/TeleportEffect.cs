using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeleportEffect : TowerSkillEffect
{

	public float teleportOffset = 1f;

	public GameObject fxPrefab;

	public float fxDuration = 3f;


	void Start()
	{
		float radius = radiusCollider.radius * Mathf.Max(radiusCollider.transform.localScale.x, radiusCollider.transform.localScale.y);
		
		Collider2D enemy = Physics2D.OverlapCircle(radiusCollider.transform.position, radius, 1 << LayerMask.NameToLayer("Enemy"));
		if (enemy != null)
		{
			if (fxPrefab != null)
			{
				
				GameObject fx = Instantiate(fxPrefab);
				fx.transform.position = enemy.gameObject.transform.position;
				Destroy(fx, fxDuration);
			}
			AiStatePatrol aiStatePatrol = enemy.gameObject.GetComponent<AiStatePatrol>();
		
			Vector2 teleportPosition = aiStatePatrol.path.GetOffsetPosition(ref aiStatePatrol.destination, aiStatePatrol.transform.position, teleportOffset);
			aiStatePatrol.transform.position = new Vector3(teleportPosition.x, teleportPosition.y, aiStatePatrol.transform.position.z);
			
			aiStatePatrol.UpdateDestination(false);
			if (fxPrefab != null)
			{
				
				GameObject fx = Instantiate(fxPrefab);
				fx.transform.position = enemy.gameObject.transform.position;
				Destroy(fx, fxDuration);
			}
		}
		Destroy(gameObject);
	}
}
