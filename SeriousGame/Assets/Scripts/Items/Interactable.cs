using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class for objects that have an effect when the player goes near and presses E
public abstract class Interactable : MonoBehaviour
{
    public abstract void OnInteract(BagManager bagManager);

    //Reference to player
    private PlayerInteract player;
    protected void Start()
    {
        //Find player object using tag
        player = GameObject.FindWithTag("Player").GetComponent<PlayerInteract>();
    }

    //When an object enters trigger area
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //If it's the player, add this pickup to list of items in range
            if (enabled) player.AddInRange(this);
        }
    }

    //When object exits trigger area
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //If it's the player, take this pickup out of their range
            player.OutOfRange(this);
        }
    }

    //Prevent player from interacting with this
    //Used when pickup is absorbed into function gate
    public void DisableInteract()
    {
        player.OutOfRange(this);
        enabled = false;
    }
}
