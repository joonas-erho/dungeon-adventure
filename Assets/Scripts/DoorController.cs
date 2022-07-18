using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int amountOfKeysNeeded;

    public Sprite[] doorSprites;

    public SpriteRenderer sr;

    public void UseKey() {
        amountOfKeysNeeded--;
        sr.sprite = doorSprites[amountOfKeysNeeded];
        if (amountOfKeysNeeded == 0) {
            Debug.Log("You win this round!!!! Ootpa hyvä");
        }
    }
}
