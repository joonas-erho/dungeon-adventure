using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject actionPrefab;
    public GameObject actionStore;
    public List<ActionScriptableObject> actions = new();
    

    void Start()
    {
        for (int i = 0; i < actions.Count; i++) {
            GameObject go = Instantiate(actionPrefab);
            go.transform.SetParent(actionStore.transform);
            go.GetComponent<ActionController>().obj = actions[i];
            go.transform.localPosition = new Vector3(i * 0.85f, 0, 0);
            go.name = "Action (" + actions[i].actionName + ")";
        }
    }
}
