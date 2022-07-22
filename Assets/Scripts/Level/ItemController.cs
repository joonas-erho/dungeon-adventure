using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles items on the level. The functionality of each item is determined in the Player.
/// </summary>
public class ItemController : MonoBehaviour
{
    public ItemScriptableObject item;
    public SpriteRenderer sr;

    void Start()
    {
        sr.sprite = item.sprite;
    }
}
