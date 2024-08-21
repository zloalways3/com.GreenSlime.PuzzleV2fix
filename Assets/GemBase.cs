using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBase : MonoBehaviour
{
    public GemType gemType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeGem(Sprite sprite, int value)
    {
        gemType = (GemType) value;
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }
}

public enum GemType
{
    Yellow, Blue, Green, Purple, None
}


