using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
	public AudioClip audioClip;


	public void ButtonPressed(string buttonName)
	{
		StartCoroutine(PressedCoroutine(buttonName));
	}

	
	private IEnumerator PressedCoroutine(string buttonName)
	{
		
		if (audioClip != null && AudioManager.instance != null)
		{
			Button button = GetComponent<Button>();
			button.interactable = false;
			AudioManager.instance.PlaySound(audioClip);
			
			yield return new WaitForSecondsRealtime(audioClip.length);
			button.interactable = true;
		}
		
		EventManager.TriggerEvent("ButtonPressed", gameObject, buttonName);
	}

	
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
