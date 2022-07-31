/// <summary>
/// Item Controller
/// Joonas Erho & Melinda Suvivirta, 31.07.2022
/// 
/// Handles items on the level. The functionality of each item is determined in the Player.
/// </summary>

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
