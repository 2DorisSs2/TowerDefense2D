using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowInfo : MonoBehaviour
{

    public Text unitName;
	
    public Image primaryIcon;
	
    public Text primaryText;
	
    public Image secondaryIcon;

    public Text secondaryText;
	
	public Sprite hitpointsIcon;
	
	public Sprite meleeAttackIcon;
	
	public Sprite rangedAttackIcon;
	
	public Sprite defendersNumberIcon;
	
	public Sprite cooldownIcon;

   
    void OnDestroy()
    {
		EventManager.StopListening("UserClick", UserClick);
    }

	
    void Awake()
    {
        Debug.Assert(unitName && primaryIcon && primaryText && secondaryIcon && secondaryText, "Wrong intial settings");
    }

	
    void Start()
    {
		EventManager.StartListening("UserClick", UserClick);
        HideUnitInfo();
    }


	public void ShowUnitInfo(UnitInfo info, GameObject obj)
    {
		if (info.unitName != "")
		{
			unitName.text = info.unitName;
		}
		else
		{
			unitName.text = obj.name;
		}

		if (info.primaryIcon != null || info.secondaryIcon != null || info.primaryText != "" || info.secondaryText != "")
		{
			primaryText.text = info.primaryText;
			secondaryText.text = info.secondaryText;

			if (info.primaryIcon != null)
			{
				primaryIcon.sprite = info.primaryIcon;
				primaryIcon.gameObject.SetActive(true);
			}

			if (info.secondaryIcon != null)
			{
				secondaryIcon.sprite = info.secondaryIcon;
				secondaryIcon.gameObject.SetActive(true);
			}
		}
		else
		{
			DamageTaker damageTaker = obj.GetComponentInChildren<DamageTaker>();
			Attack attack = obj.GetComponentInChildren<Attack>();
			DefendersSpawner spawner = obj.GetComponentInChildren<DefendersSpawner>();

			
			if (damageTaker != null)
			{
				primaryText.text = damageTaker.hitpoints.ToString();
				primaryIcon.sprite = hitpointsIcon;
				primaryIcon.gameObject.SetActive(true);
			}
			else
			{
				if (attack != null)
				{
					if (attack != null)
					{
						primaryText.text = attack.cooldown.ToString();
						primaryIcon.sprite = cooldownIcon;
						primaryIcon.gameObject.SetActive(true);
					}
				}
				else if (spawner != null)
				{
					primaryText.text = spawner.cooldown.ToString();
					primaryIcon.sprite = cooldownIcon;
					primaryIcon.gameObject.SetActive(true);
				}
			}

			if (attack != null)
			{
				secondaryText.text = attack.damage.ToString();
				if (attack is AttackMelee)
				{
					secondaryIcon.sprite = meleeAttackIcon;
				}
				else if (attack is AttackRanged)
				{
					secondaryIcon.sprite = rangedAttackIcon;
				}
				secondaryIcon.gameObject.SetActive(true);
			}
			else
			{
				if (spawner != null)
				{
					secondaryText.text = spawner.maxNum.ToString();
					secondaryIcon.sprite = defendersNumberIcon;
					secondaryIcon.gameObject.SetActive(true);
				}
			}
		}
		gameObject.SetActive(true);
    }


    public void HideUnitInfo()
    {
        unitName.text = primaryText.text = secondaryText.text = "";
        primaryIcon.gameObject.SetActive(false);
        secondaryIcon.gameObject.SetActive(false);
		gameObject.SetActive(false);
    }


    private void UserClick(GameObject obj, string param)
    {
        HideUnitInfo();
        if (obj != null)
        {
			
			UnitInfo unitInfo = obj.GetComponentInChildren<UnitInfo>();
            if (unitInfo != null)
            {
				ShowUnitInfo(unitInfo, obj);
            }
        }
    }
}
