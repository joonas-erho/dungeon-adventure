using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Values that are only edited in editor.
    public float timeBetweenActions = 0.5f;
    public float speed = 5f;

    private bool isMoving = false;
    private Vector2 moveTargetLocation;

    void Update() {
        // If we are supposed to be moving, move player towards target location.
        // Step is the max distance it can move per frame, controlled by the "speed" variable.
        if (isMoving) {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, moveTargetLocation, step);
        }
    }

    /// <summary>
    /// Executes the given actions.
    /// </summary>
    /// <param name="actions">A list of strings that correspond to different actions in Execute().</param>
    public IEnumerator ExecuteActions(List<string> actions) {
        foreach (string a in actions) {
            Execute(a);
            yield return new WaitForSeconds(timeBetweenActions);
            isMoving = false;
        }
    }

    /// <summary>
    /// Performs the given action (string). Logs errors, but this should never happen at runtime.
    /// </summary>
    /// <param name="action">String that corresponds to an action.</param>
    private void Execute(string action) {
        switch(action) {
            case "movel":
                Move(-1,0);
                break;
            case "moveu":
                Move(0,1);
                break;
            case "mover":
                Move(1,0);
                break;
            case "moved":
                Move(0,-1);
                break;
            default:
                // This should never happen in-game!
                Debug.Log("Such action does not exist!");
                break;
        }
    }

    /// <summary>
    /// Makes the player move smoothly to the given position.
    /// </summary>
    /// <param name="x">Player position change on the x-axis.</param>
    /// <param name="y">Player position change on the y-axis.</param>
    private void Move(int x, int y) {
        // Change the location that the player is supposed to move to by taking player's current
        // position and adding x and y to it. (For example, when moving left, add -1 and 0.)
        moveTargetLocation = new Vector2(this.transform.position.x + x, this.transform.position.y + y);

        // Begin movement by enabling movement flag.
        isMoving = true;
    }
}
