using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public ActionScriptableObject obj;
    public SpriteRenderer sr;

    void Start() {
        sr.color = obj.color;
    }
}
