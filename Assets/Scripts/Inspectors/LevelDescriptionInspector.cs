using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class LevelDescriptionInspector : MonoBehaviour
{

	public Image icon;

	public Text header;

	public Text description;

	public Text attention;

	void OnEnable()
	{
		Debug.Assert(icon && header && description && attention, "Wrong level description stuff settings");
	}
}
#endif
