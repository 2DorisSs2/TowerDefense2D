using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TowerActionCooldown : TowerAction
{
	
	public float cooldown = 10f;
	
	public GameObject cooldownIcon;
	
	public Text cooldownText;


	private enum MyState
	{
		Active,
		Cooldown
	}
	
	private MyState myState = MyState.Active;
	
	private float cooldownStartTime;

	
	void Awake()
	{
		Debug.Assert(cooldownIcon && cooldownText, "Wrong initial settings");
		StopCooldown();
	}

	
	void Update()
	{
		if (myState == MyState.Cooldown)
		{
			float cooldownCounter = Time.time - cooldownStartTime;
			if (cooldownCounter < cooldown)
			{
				UpdateCooldownText(cooldown - cooldownCounter);
			}
			else
			{
				StopCooldown();
			}
		}
	}

	
	private void StartCooldown()
	{
		myState = MyState.Cooldown;
		cooldownStartTime = Time.time;
		enabledIcon.SetActive(false);
		cooldownIcon.gameObject.SetActive(true);
		cooldownText.gameObject.SetActive(true);
	}

	
	private void StopCooldown()
	{
		myState = MyState.Active;
		enabledIcon.SetActive(true);
		cooldownIcon.gameObject.SetActive(false);
		cooldownText.gameObject.SetActive(false);
	}

	private void UpdateCooldownText(float cooldownCounter)
	{
		cooldownText.text = ((int)Mathf.Ceil(cooldownCounter)).ToString();
	}

	
	protected override void Clicked()
	{
		StartCooldown();
	}
}
