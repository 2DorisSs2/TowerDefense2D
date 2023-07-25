using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class UiManager : MonoBehaviour
{
   
    public string exitSceneName;

	public GameObject startScreen;
  
    public GameObject pauseMenu;
   
    public GameObject defeatMenu;
   
    public GameObject victoryMenu;
   
    public GameObject levelUI;
   
    public Text goldAmount;
	
	public Text defeatAttempts;
	
	public float menuDisplayDelay = 1f;

 
    private bool paused;
 
    private bool cameraIsDragged;
   
    private Vector3 dragOrigin = Vector3.zero;
   
    private CameraControl cameraControl;

	
	void Awake()
	{
		cameraControl = FindObjectOfType<CameraControl>();
		Debug.Assert(cameraControl && startScreen && pauseMenu && defeatMenu && victoryMenu && levelUI && defeatAttempts && goldAmount, "Wrong initial parameters");
	}

    
    void OnEnable()
    {
		EventManager.StartListening("UnitKilled", UnitKilled);
		EventManager.StartListening("ButtonPressed", ButtonPressed);
		EventManager.StartListening("Defeat", Defeat);
		EventManager.StartListening("Victory", Victory);
    }

    
    void OnDisable()
    {
		EventManager.StopListening("UnitKilled", UnitKilled);
		EventManager.StopListening("ButtonPressed", ButtonPressed);
		EventManager.StopListening("Defeat", Defeat);
		EventManager.StopListening("Victory", Victory);
    }

    
    void Start()
    {
		PauseGame(true);
    }

   
    void Update()
    {
        if (paused == false)
        {
            
            if (Input.GetMouseButtonDown(0) == true)
            {
              
                GameObject hittedObj = null;
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);
				if (results.Count > 0) 
				{
					
					foreach (RaycastResult res in results)
					{
						if (res.gameObject.CompareTag("ActionIcon"))
						{
							hittedObj = res.gameObject;
							break;
						}
					}
					
					EventManager.TriggerEvent("UserUiClick", hittedObj, null);
				}
				else 
                {
                   
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
                    foreach (RaycastHit2D hit in hits)
                    {
                        
                        if (hit.collider.CompareTag("UnitInfo"))
                        {
							Tower tower = hit.collider.GetComponentInParent<Tower>();
							if (tower != null)
							{
								hittedObj = tower.gameObject;
								break;
							}
							AiBehavior aiBehavior = hit.collider.GetComponentInParent<AiBehavior>();
							if (aiBehavior != null)
							{
								hittedObj = aiBehavior.gameObject;
								break;
							}
							hittedObj = hit.collider.gameObject;
                            break;
                        }
                    }
					
					EventManager.TriggerEvent("UserClick", hittedObj, null);
                }
				
                if (hittedObj == null)
                {
                    cameraIsDragged = true;
                    dragOrigin = Input.mousePosition;
                }
            }
            if (Input.GetMouseButtonUp(0) == true)
            {
				
                cameraIsDragged = false;
            }
            if (cameraIsDragged == true)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
				
                cameraControl.MoveX(-pos.x);
                cameraControl.MoveY(-pos.y);
            }
        }
    }

    

    private void LoadScene(string sceneName)
    {
		EventManager.TriggerEvent("SceneQuit", null, null);
        SceneManager.LoadScene(sceneName);
    }

  
	private void ResumeGame()
    {
        GoToLevel();
        PauseGame(false);
    }

    
	private void ExitFromLevel()
    {
        LoadScene(exitSceneName);
    }

   
    private void CloseAllUI()
    {
		startScreen.SetActive (false);
        pauseMenu.SetActive(false);
        defeatMenu.SetActive(false);
        victoryMenu.SetActive(false);
    }


    private void PauseGame(bool pause)
    {
        paused = pause;
 
        Time.timeScale = pause ? 0f : 1f;
		EventManager.TriggerEvent("GamePaused", null, pause.ToString());
    }


	private void GoToPauseMenu()
    {
        PauseGame(true);
        CloseAllUI();
        pauseMenu.SetActive(true);
    }


    private void GoToLevel()
    {
        CloseAllUI();
        levelUI.SetActive(true);
        PauseGame(false);
    }

	private void Defeat(GameObject obj, string param)
    {
		StartCoroutine("DefeatCoroutine");
    }

	
	private IEnumerator DefeatCoroutine()
	{
		yield return new WaitForSeconds(menuDisplayDelay);
		PauseGame(true);
		CloseAllUI();
		defeatMenu.SetActive(true);
	}

   
	private void Victory(GameObject obj, string param)
    {
		StartCoroutine("VictoryCoroutine");
    }

	private IEnumerator VictoryCoroutine()
	{
		yield return new WaitForSeconds(menuDisplayDelay);
		PauseGame(true);
		CloseAllUI();

		
		DataManager.instance.progress.lastCompetedLevel = SceneManager.GetActiveScene().name;
		
		bool hit = false;
		foreach (string level in DataManager.instance.progress.openedLevels)
		{
			if (level == SceneManager.GetActiveScene().name)
			{
				hit = true;
				break;
			}
		}
		if (hit == false)
		{
			DataManager.instance.progress.openedLevels.Add(SceneManager.GetActiveScene().name);
		}
		
		DataManager.instance.SaveGameProgress();

		victoryMenu.SetActive(true);
	}

    
	private void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

	public int GetGold()
    {
        int gold;
        int.TryParse(goldAmount.text, out gold);
        return gold;
    }

   
	public void SetGold(int gold)
    {
        goldAmount.text = gold.ToString();
    }

    
	public void AddGold(int gold)
    {
        SetGold(GetGold() + gold);
    }

    
    public bool SpendGold(int cost)
    {
        bool res = false;
        int currentGold = GetGold();
        if (currentGold >= cost)
        {
            SetGold(currentGold - cost);
            res = true;
        }
        return res;
    }

	
	public void SetDefeatAttempts(int attempts)
	{
		defeatAttempts.text = attempts.ToString();
	}

    
	private void UnitKilled(GameObject obj, string param)
    {
       
		if (obj.CompareTag("Enemy") || obj.CompareTag("FlyingEnemy"))
        {
            Price price = obj.GetComponent<Price>();
            if (price != null)
            {
                
                AddGold(price.price);
            }
        }
    }

	
	private void ButtonPressed(GameObject obj, string param)
	{
		switch (param)
		{
		case "Pause":
			GoToPauseMenu();
			break;
		case "Resume":
			GoToLevel();
			break;
		case "Back":
			ExitFromLevel();
			break;
		case "Restart":
			RestartLevel();
			break;
		}
	}

	
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
