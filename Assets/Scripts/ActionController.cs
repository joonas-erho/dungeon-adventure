using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public ActionScriptableObject obj;
    public SpriteRenderer sr;
    public SpriteRenderer iconSr;

    void Start() {
        sr.color = obj.color;
        iconSr.sprite = obj.icon;
    }

    void OnMouseDown() {
        Debug.Log(obj.name);
    }
}
