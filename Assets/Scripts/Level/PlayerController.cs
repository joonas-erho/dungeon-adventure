/// <summary>
/// Player Controller
/// Joonas Erho & Melinda Suvivirta, 31.07.2022
/// 
/// Controls the player character in the level.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    // Values that are only edited in editor.
    public float timeBetweenActions = 0.5f;
    public float speed = 5f;
    private float swingTime = 0.35f;

    // Collider of player, and additional colliders that exist in cardinal directions.
    // These additional colliders are used to check for walls.
    public GameController gameController;
    public BoxCollider2D leftCollider;
    public BoxCollider2D rightCollider;
    public BoxCollider2D topCollider;
    public BoxCollider2D bottomCollider;

    // Reference to the collider of the walls, used for wall collision checking.
    public TilemapCollider2D wallsCollider;

    public SpriteRenderer sr;
    
    // Right now we have an audio source for each sound. Later this could be edited so
    // that there is only one audio source and the audio clip changed depending on the
    // one needed.
    public AudioSource doorOpen;
    public AudioSource keyClink;
    public AudioSource footsteps;
    public AudioSource swordPickup;
    public AudioSource swordMiss;
    public AudioSource swordHit;

    // The sprite of the player is changed to this when the sword is picked up.
    public Sprite spriteWithSword;

    // The gameobject that is spawned when the player swings their sword.
    public GameObject swingAnimPrefab;

    // The location that the player will next move to.
    private Vector2 moveTargetLocation;

    // Items the player has collected this level. (Does not include the sword.)
    private ItemScriptableObject[] itemsInInventory = new ItemScriptableObject[3];

    // Flags
    private bool isMoving = false;
    private bool hasSword = false;

    // Counters for this level.
    private int treasuresCollected;
    private int monstersKilled;

    public int GetTreasuresCollected() {
        return treasuresCollected;
    }

    public int GetMonstersKilled() {
        return monstersKilled;
    }

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
    public void Execute(string action) {
        switch(action) {
            case "moveleft":
                Move(-1,0,leftCollider);
                break;
            case "moveright":
                Move(1,0,rightCollider);
                break;
            case "moveup":
                Move(0,1,topCollider);
                break;
            case "movedown":
                Move(0,-1,bottomCollider);
                break;
            case "pickup":
                PickupItem();
                break;
            case "useitem0":
                UseItem(0);
                break;
            case "useitem1":
                UseItem(1);
                break;
            case "useitem2":
                UseItem(2);
                break;
            case "wait":
                // Do nothing; wait for next cycle.
                break;
            case "swingleft":
                Swing(-0.4f, 0, 180, leftCollider);
                break;
            case "swingright":
                Swing(0.4f, 0, 0, rightCollider);
                break;
            case "swingup":
                Swing(0, 0.5f, 90, topCollider);
                break;
            case "swingdown":
                Swing(0, -0.3f, -90, bottomCollider);
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
    /// <param name="col">The collider that checks if it hits a wall.</param>
    private void Move(int x, int y, BoxCollider2D col) {
        // If the collider in the given direction hits a wall, don't move.
        if (Physics2D.IsTouching(col, wallsCollider)) {
            return;
        }
        // Change the location that the player is supposed to move to by taking player's current
        // position and adding x and y to it. (For example, when moving left, add -1 and 0.)
        moveTargetLocation = new Vector2(this.transform.position.x + x, this.transform.position.y + y);

        // Begin movement by enabling movement flag.
        isMoving = true;
        footsteps.pitch = Random.Range(0.75f,1.25f);
        footsteps.Play();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // If we hit a monster, the player "dies" and the level is going to be reset.
        if (other.tag == "Monster") {
            gameController.queueShouldBeStopped = true;

            // Temporary since there is no death animation implemented yet.
            sr.sprite = null;
        }

        // If we are on the door, and all locks are opened, the level is won.
        if (other.tag == "Door") {
            DoorController doorController = other.gameObject.GetComponent<DoorController>();
            if (doorController.amountOfKeysNeeded == 0) {
                StartCoroutine(WinLevel());
            }
        }
    }

    /// <summary>
    /// Called when the player executes the "Pickup" command. If player is not in the same tile as
    /// an item, nothing happens. Otherwise the player will pickup the item, unless it's not possible.
    /// </summary>
    private void PickupItem() {
        // Get all collisions on the current position of the player.
        Collider2D[] collisions = Physics2D.OverlapCircleAll(this.transform.position, 0.1f);

        // Iterate through them to find an item. If no item is found, nothing happens.
        foreach (Collider2D other in collisions) {
            if (other.tag == "Item") {
                ItemScriptableObject item = other.gameObject.GetComponent<ItemController>().item;

                // If the item is the sword, equip it.
                if (item.itemName == "sword") {
                    gameController.ToggleSwordVisibility(true);
                    hasSword = true;
                    swordPickup.Play();
                    Destroy(other.gameObject);
                    sr.sprite = spriteWithSword;
                    return;
                }

                // Otherwise see if there is room in the inventory, and if so, pick up the item.
                for (int i = 0; i < itemsInInventory.Length; i++) {
                    if (itemsInInventory[i] == null) {
                        itemsInInventory[i] = item;
                        gameController.inventoryRenderers[i].sprite = item.sprite;
                        Destroy(other.gameObject);
                        keyClink.Play();

                        if (item.itemName == "gem") {
                            treasuresCollected++;
                        }
                        break;
                    }
                } 
            }
        }
    }
    
    /// <summary>
    /// Uses the item in the given inventory slot. Right now only has functionality for keys and the door.
    /// </summary>
    /// <param name="index"></param>
    private void UseItem(int index) {
        // If the given inventory slot has no item, return.
        if (itemsInInventory[index] == null) {
            return;
        }

        // Otherwise check if we on a door and if the given inventory slot has a key, use it.
        Collider2D[] collisions = Physics2D.OverlapCircleAll(this.transform.position, 0.1f);
        foreach (Collider2D other in collisions) {
            if (other.tag == "Door" && itemsInInventory[index].itemName == "key") {
                DoorController doorController = other.gameObject.GetComponent<DoorController>();
                gameController.inventoryRenderers[index].sprite = null;
                itemsInInventory[index] = null;

                // If the key was the last one that needs to be used, win the level.
                if (doorController.UseKey() == 0) {
                    StartCoroutine(WinLevel());
                }
            }
        }
    }

    /// <summary>
    /// Makes the player swing their sword in the given direction.
    /// </summary>
    /// <param name="x">x position of swing.</param>
    /// <param name="y">y position of swing.</param>
    /// <param name="angle">The angle of swing (this is used only to orient the animation correctly).</param>
    /// <param name="col">The collider that will check if an enemy was in the direction swung at.</param>
    private void Swing(float x, float y, int angle, BoxCollider2D col) {
        // If the player does not have a sword, return.
        if (!hasSword) {
            return;
        }

        // Calculate the position of the swing by adding the given x and y to player position.
        Vector2 posOfSwing = this.transform.position;
        posOfSwing.x += x;
        posOfSwing.y += y;

        // Instantiate the object, also using the correct angle. The object is destroyed after a short while.
        GameObject go = Instantiate(swingAnimPrefab, posOfSwing, Quaternion.Euler(new Vector3(0, 0, angle)));
        Destroy(go, swingTime);

        // Check for collisions in the direction. If monsters are found, destroy those monsters.
        Collider2D[] collisions = Physics2D.OverlapCircleAll(col.gameObject.transform.position, 0.1f);
        foreach (Collider2D other in collisions) {
            if (other.tag == "Monster") {
                gameController.DestroyMonster(other.gameObject);
                monstersKilled++;
                swordHit.Play();
            }
            else {
                swordMiss.Play();
            }
        }
    }

    /// <summary>
    /// Called when the level is won. Plays the door opening sound, and after a short delay, calls the GameController
    /// to open the victory screen and block user input.
    /// </summary>
    private IEnumerator WinLevel() {
        doorOpen.Play();
        yield return new WaitForSeconds(1.25f);
        gameController.WinLevel();
    }
}
