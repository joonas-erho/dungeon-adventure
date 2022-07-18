using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemScriptableObject item;
    public SpriteRenderer sr;

    void Start()
    {
        sr.sprite = item.sprite;
    }

}
