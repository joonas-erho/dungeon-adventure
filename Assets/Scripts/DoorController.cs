using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int amountOfKeysNeeded;

    public Sprite[] doorSprites;

    public GameController gameController;

    public SpriteRenderer sr;

    public int UseKey() {
        amountOfKeysNeeded--;
        sr.sprite = doorSprites[amountOfKeysNeeded];
        return amountOfKeysNeeded;
    }
}
