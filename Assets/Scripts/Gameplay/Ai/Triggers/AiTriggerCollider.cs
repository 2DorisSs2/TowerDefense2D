using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTriggerCollider : MonoBehaviour
{

    public List<string> tags = new List<string>();


    private Collider2D col;

    private AiBehavior aiBehavior;

  
    void Awake()
    {
        col = GetComponent<Collider2D>();
        aiBehavior = GetComponentInParent<AiBehavior>();
        Debug.Assert(col && aiBehavior, "Wrong initial parameters");
		col.enabled = false;
    }


	void Start()
	{
		col.enabled = true;
	}

    private bool IsTagAllowed(string tag)
    {
        bool res = false;
        if (tags.Count > 0)
        {
            foreach (string str in tags)
            {
                if (str == tag)
                {
                    res = true;
                    break;
                }
            }
        }
        else
        {
            res = true;
        }
        return res;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            
			aiBehavior.OnTrigger(AiState.Trigger.TriggerEnter, col, other);
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
         
			aiBehavior.OnTrigger(AiState.Trigger.TriggerStay, col, other);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
          
			aiBehavior.OnTrigger(AiState.Trigger.TriggerExit, col, other);
        }
    }
}
