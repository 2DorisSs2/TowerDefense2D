using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tower : MonoBehaviour
{

	public GameObject actions;
    
    public GameObject range;

   
    private UiManager uiManager;

  
    void OnEnable()
    {
        EventManager.StartListening("GamePaused", GamePaused);
        EventManager.StartListening("UserClick", UserClick);
		EventManager.StartListening("UserUiClick", UserClick);
    }

   
    void OnDisable()
    {
        EventManager.StopListening("GamePaused", GamePaused);
        EventManager.StopListening("UserClick", UserClick);
		EventManager.StopListening("UserUiClick", UserClick);
    }

    
    void Start()
    {
        uiManager = FindObjectOfType<UiManager>();
		Debug.Assert(uiManager && actions, "Wrong initial parameters");
		CloseActions();
    }

  
    private void OpenActions()
    {
		actions.SetActive(true);
    }

    
    private void CloseActions()
    {
		if (actions.activeSelf == true)
        {
			actions.SetActive(false);
        }
    }

    
    public void BuildTower(GameObject towerPrefab)
    {
        
        CloseActions();
        Price price = towerPrefab.GetComponent<Price>();
       
        if (uiManager.SpendGold(price.price) == true)
        {
            
            GameObject newTower = Instantiate<GameObject>(towerPrefab, transform.parent);
			newTower.name = towerPrefab.name;
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
            
            Destroy(gameObject);
			EventManager.TriggerEvent("TowerBuild", newTower, null);
        }
    }

	
	public void SellTower(GameObject emptyPlacePrefab)
	{
		CloseActions();
		DefendersSpawner defendersSpawner = GetComponent<DefendersSpawner>();
		
		if (defendersSpawner != null)
		{
			foreach (KeyValuePair<GameObject, Transform> pair in defendersSpawner.defPoint.activeDefenders)
			{
				Destroy(pair.Key);
			}
		}
		Price price = GetComponent<Price>();
		uiManager.AddGold(price.price / 2);
		
		GameObject newTower = Instantiate<GameObject>(emptyPlacePrefab, transform.parent);
		newTower.name = emptyPlacePrefab.name;
		newTower.transform.position = transform.position;
		newTower.transform.rotation = transform.rotation;
		
		Destroy(gameObject);
		EventManager.TriggerEvent("TowerSell", null, null);
	}

   
    private void GamePaused(GameObject obj, string param)
    {
        if (param == bool.TrueString) 
        {
            CloseActions();
        }
    }

    
    private void UserClick(GameObject obj, string param)
    {
        if (obj == gameObject) 
        {
            
			ShowRange(true);
			if (actions.activeSelf == false)
            {
                
                OpenActions();
            }
        }
        else 
        {
            
			ShowRange(false);
          
            CloseActions();
        }
    }

   
	public void ShowRange(bool condition)
    {
        if (range != null)
        {
			range.SetActive(condition);
        }
    }
}
