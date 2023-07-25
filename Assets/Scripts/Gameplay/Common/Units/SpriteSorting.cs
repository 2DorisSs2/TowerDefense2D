using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteSorting : MonoBehaviour
{
    
    public bool isStatic;
  
    public float rangeFactor = 100f;

    
    private Dictionary<SpriteRenderer, int> sprites = new Dictionary<SpriteRenderer, int>();

    void Awake()
    {
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprites.Add(sprite, sprite.sortingOrder);
        }
    }

   
    void Start()
    {
        UpdateSortingOrder();
    }

  
    void Update()
    {
        if (isStatic == false)
        {
            UpdateSortingOrder();
        }
    }

    
    private void UpdateSortingOrder()
    {
        foreach (KeyValuePair<SpriteRenderer, int> sprite in sprites)
        {
            sprite.Key.sortingOrder = sprite.Value - (int)(transform.position.y * rangeFactor);
        }
    }
}
