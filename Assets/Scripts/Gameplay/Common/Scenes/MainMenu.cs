using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public string startSceneName;

	public GameObject creditsMenu;


	void OnEnable()
	{
		EventManager.StartListening("ButtonPressed", ButtonPressed);
	}


	void OnDisable()
	{
		EventManager.StopListening("ButtonPressed", ButtonPressed);
	}

	void Awake()
	{
		Debug.Assert(creditsMenu, "Wrong initial settings");
	}



	private void ButtonPressed(GameObject obj, string param)
	{
		switch (param)
		{
		case "Quit":
			Application.Quit();
			break;
		case "Start":
			SceneManager.LoadScene(startSceneName);
			break;
		case "OpenCredits":
			creditsMenu.SetActive(true);
			break;
		case "CloseCredits":
			creditsMenu.SetActive(false);
			break;
		}
	}
}
