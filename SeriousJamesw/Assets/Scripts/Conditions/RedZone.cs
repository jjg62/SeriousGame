using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//A zone that should prevent player passage unless a certain condition is met
public class RedZone : HasDisplay, IPointerEnterHandler, IPointerExitHandler, IInventoryCheck
{
    //Get reference to sprite
    private SpriteRenderer spr;

    //Collider to stop movement
    [SerializeField]
    private Collider2D col;

    //Colour when condition failed
    [SerializeField]
    private Color blockedColour;

    //Colour when condition passed
    [SerializeField]
    private Color passColour;

    //When object is created, before start
    private new void Awake()
    {
        base.Awake();
        spr = GetComponent<SpriteRenderer>();
        
        col.enabled = false; //Collider disabled by default
    }

    //When an object enters trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") //Check the object is the player
        {
            //Get player's BagManager
            BagManager bags = collision.gameObject.GetComponent<BagManager>();
            bags.inRedZone = this; //Player is in this red zone

            //If the player tries to enter without passing condition, alert them
            if (!con.Test())
            {
                display.Flash();
                AudioManager.instance.Play("Fail");
            }
        }
    }

    //When an object exits trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") //Check it's the player
        {
            //Reset inRedZone if it previously was this one
            BagManager bags = collision.gameObject.GetComponent<BagManager>();
            if (bags.inRedZone == this) bags.inRedZone  = null;
        }
    }

    //Called whenever conditions are updated, e.g. when an inventory is changed
    public void InventoryCheck(Inventory inv)
    {
        //If condition is passed, disable the collider
        if (con.Test())
        {
            col.enabled = false;
            spr.color = passColour;
        }
        else
        {
            //Otherwise enable it
            col.enabled = true;
            spr.color = blockedColour;
        }
    }
}
