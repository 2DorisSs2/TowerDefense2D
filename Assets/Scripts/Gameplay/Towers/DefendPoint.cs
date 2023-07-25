using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefendPoint : MonoBehaviour
{
	
	public GameObject defendPointPrefab;

	public GameObject defendFlagPrefab;

	public bool clockwiseDirection = false;

	
	public Dictionary<GameObject, Transform> activeDefenders = new Dictionary<GameObject, Transform>();


	private GameObject activeDefendFlag;

	private List<Transform> defendPlaces = new List<Transform>();

	private enum MyState
	{
		Inactive,
		Active
	}

	private MyState myState = MyState.Inactive;

	private BuildingPlace buildingPlace;

	private Tower tower;


	void OnEnable()
	{
		EventManager.StartListening("UserClick", UserClick);
		EventManager.StartListening("UserUiClick", UserUiClick);
		EventManager.StartListening("UnitDie", UnitDie);
	}


	void OnDisable()
	{
		EventManager.StopListening("UserClick", UserClick);
		EventManager.StopListening("UserUiClick", UserUiClick);
		EventManager.StopListening("UnitDie", UnitDie);
	}


	void Awake()
	{
		Debug.Assert(defendPointPrefab && defendFlagPrefab, "Wrong initial settings");
		
		foreach (Transform defendPlace in defendPointPrefab.transform)
		{
			Instantiate(defendPlace.gameObject, transform);
		}
		
		foreach (Transform child in transform)
		{
			defendPlaces.Add(child);
		}
		BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
		LookAtDirection2D((Vector2)(buildingPlace.transform.position - transform.position));
	}

  
    public List<Transform> GetDefendPoints()
    {
		return defendPlaces;
    }

	
	public void SetVisible(bool enabled)
	{
		if (enabled == true)
		{
			if (myState == MyState.Inactive)
			{
				buildingPlace = GetComponentInParent<BuildingPlace>();
				tower = buildingPlace.GetComponentInChildren<Tower>();
				
				activeDefendFlag = Instantiate(defendFlagPrefab);
				activeDefendFlag.transform.position = transform.position;
				myState = MyState.Active;
			}
		}
		else
		{
			if (myState == MyState.Active)
			{
				myState = MyState.Inactive;
				
				tower.ShowRange(false);
			
				Destroy(activeDefendFlag);
			}
		}
	}

	
	private void UserUiClick(GameObject obj, string param)
	{
		if (myState == MyState.Active)
		{
			SetVisible(false);
		}
	}

	
	private void UserClick(GameObject obj, string param)
	{
		if (myState == MyState.Active)
		{
			myState = MyState.Inactive;
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 delta = position - (Vector2)tower.transform.position;
			delta = Vector2.ClampMagnitude(delta, tower.range.transform.localScale.x);
			transform.position = tower.transform.position + (Vector3)delta;
			LookAtDirection2D((Vector2)(tower.transform.position - transform.position));
			SetVisible(false);
			
			foreach (KeyValuePair<GameObject, Transform> pair in activeDefenders)
			{
				AiBehavior aiBehavior = pair.Key.GetComponent<AiBehavior>();
				aiBehavior.ChangeState(aiBehavior.GetComponent<AiStateMove>());
			}
			Destroy(activeDefendFlag);
		}
	}

	
	private void UnitDie(GameObject obj, string param)
	{
	
		if (activeDefenders.ContainsKey(obj) == true)
		{
			
			activeDefenders.Remove(obj);
		}
	}


	public List<GameObject> GetDefenderList()
	{
		List<GameObject> res = new List<GameObject>();
		foreach (KeyValuePair<GameObject, Transform> pair in activeDefenders)
		{
			res.Add(pair.Key);
		}
		return res;
	}

	
	public Transform GetFreeDefendPosition()
	{
		Transform res = null;
		List<Transform> points = GetDefendPoints();
		foreach (Transform point in points)
		{
		
			if (activeDefenders.ContainsValue(point) == false)
			{
				res = point;
				break;
			}
		}
		return res;
	}

	
	private void LookAtDirection2D(Vector2 direction)
	{
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		float offset = clockwiseDirection == false ? 90f : -90f;
		transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
	}
}
