using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Prefab for the Action boxes.
    public GameObject actionPrefab;

    // The field that holds all the Actions that are available in this level.
    public GameObject actionStore;

    // The field that the player can drag Actions to.
    public GameObject actionQueue;

    // List of all actions. (Used mainly editor-side)
    public List<ActionScriptableObject> actions = new();
    

    void Start() {
        GenerateAvailableActions(actions);
    }

    /// <summary>
    /// Generates available actions and places them as children to the "Action Store"
    /// gameobject.
    /// </summary>
    /// <param name="list">List of actions as premade scriptable objects. (See Assets/Objects-folder.)</param>
    void GenerateAvailableActions(List<ActionScriptableObject> list) {
        for (int i = 0; i < list.Count; i++) {
            // Create game object and add as child to action store in correct position.
            GameObject go = Instantiate(actionPrefab);
            go.transform.SetParent(actionStore.transform);
            go.transform.localPosition = new Vector3(i * 0.85f, 0, 0);

            // Set correct name (visible in editor only).
            go.name = "Action (" + list[i].actionName + ")";
            
            // Set the object of the created prefab as the one given in the list.
            // See ActionController for more information.
            go.GetComponent<ActionController>().obj = list[i];
        }
    }
}
