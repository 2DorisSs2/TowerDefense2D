using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AloneSpeedUp : AiFeature
{

	public float speedUpAmount = 2f;

	private EffectControl effectControl;


	void Start()
	{
		effectControl = GetComponentInParent<EffectControl>();
		Debug.Assert(effectControl, "Wrong initial settings");
	}

	public void OnTriggerAloneStart()
	{
		effectControl.AddConstantEffect("Speed", speedUpAmount, null);
	}

	public void OnTriggerAloneEnd()
	{
		effectControl.RemoveConstantEffect("Speed", speedUpAmount);
	}
}
