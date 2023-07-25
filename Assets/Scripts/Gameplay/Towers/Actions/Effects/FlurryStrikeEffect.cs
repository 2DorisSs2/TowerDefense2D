using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlurryStrikeEffect : TowerSkillEffect
{

	public GameObject bulletPrefab;


	void Start()
	{
		Debug.Assert(bulletPrefab, "Wrong initial settings");
		AttackRanged attack = tower.GetComponentInChildren<AttackRanged>();
		if (attack != null)
		{
			float radius = radiusCollider.radius * Mathf.Max(radiusCollider.transform.localScale.x, radiusCollider.transform.localScale.y);
			
			Collider2D[] enemies = Physics2D.OverlapCircleAll(radiusCollider.transform.position, radius, 1 << LayerMask.NameToLayer("Enemy"));
			GameObject defaultBulletPrefab = attack.arrowPrefab;
			attack.arrowPrefab = bulletPrefab;
			foreach (Collider2D enemy in enemies)
			{
				
				attack.Fire(enemy.gameObject.transform);
			}
			attack.arrowPrefab = defaultBulletPrefab;
		}
		else
		{
			Debug.Log("This tower can not use attack skills");
		}
		Destroy(gameObject);
	}
}
