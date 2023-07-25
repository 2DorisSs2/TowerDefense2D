using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerActionSkill : TowerActionCooldown
{
	
	public TowerSkillEffect effectPrefab;

	[HideInInspector]
	public CircleCollider2D radiusCollider;

	
	private Tower tower;

	

	void Awake()
	{
		tower = GetComponentInParent<Tower>();
		Debug.Assert(effectPrefab && tower, "Wrong initial settings");
	}

	protected override void Clicked()
	{
		base.Clicked();
		
		TowerSkillEffect towerSkillEffect = Instantiate(effectPrefab);
		towerSkillEffect.tower = tower;
		AttackRanged attackRanged = tower.GetComponentInChildren<AttackRanged>();
		if (attackRanged != null)
		{
			towerSkillEffect.radiusCollider = attackRanged.GetComponent<CircleCollider2D>();
		}
		else if (tower.range != null)
		{
			towerSkillEffect.radiusCollider = tower.range.GetComponent<CircleCollider2D>();
		}
	}
}
