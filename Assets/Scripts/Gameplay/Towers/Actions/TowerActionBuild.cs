using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TowerActionBuild : TowerAction
{
    
    public GameObject towerPrefab;

	public GameObject disabledIcon;
	
	public GameObject blockedIcon;

   
	private Text priceText;
	
	private int price = 0;
	
	private LevelManager levelManager;
	
	private UiManager uiManager;

   
    void Awake()
    {
        priceText = GetComponentInChildren<Text>();
		levelManager = FindObjectOfType<LevelManager>();
		uiManager = FindObjectOfType<UiManager>();
		Debug.Assert(priceText && towerPrefab && enabledIcon && disabledIcon && levelManager && uiManager, "Wrong initial parameters");
        
		price = towerPrefab.GetComponent<Price>().price;
		priceText.text = price.ToString();
		if (levelManager.allowedTowers.Contains(towerPrefab) == true)
		{
			enabledIcon.SetActive(true);
			disabledIcon.SetActive(false);
		}
		else
		{
			enabledIcon.SetActive(false);
			disabledIcon.SetActive(true);
		}
    }

	
	void Update()
	{
		
		if (enabledIcon == true && blockedIcon != null)
		{
			if (uiManager.GetGold() >= price)
			{
				blockedIcon.SetActive(false);
			}
			else
			{
				blockedIcon.SetActive(true);
			}
		}
	}

	
	protected override void Clicked()
	{
		
		if (blockedIcon == null || blockedIcon.activeSelf == false)
		{
		
			Tower tower = GetComponentInParent<Tower>();
			if (tower != null)
			{
				tower.BuildTower(towerPrefab);
			}
		}
	}
}
