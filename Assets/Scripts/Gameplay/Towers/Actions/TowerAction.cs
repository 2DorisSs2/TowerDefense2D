using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAction : MonoBehaviour
{
	
	public GameObject enabledIcon;


	void OnEnable()
	{
		EventManager.StartListening("UserUiClick", UserUiClick);
	}

	void OnDisable()
	{
		EventManager.StopListening("UserUiClick", UserUiClick);
	}


	private void UserUiClick(GameObject obj, string param)
	{
		
		if (obj == gameObject)
		{
			if (enabledIcon.activeSelf == true)
			{
				Clicked();
			}
		}
	}

	protected virtual void Clicked()
	{

	}
}
