using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for item drops in world
public class ItemPickup : Pickup
{
    //Type of the item
    public Item.Type itemType;

    //When picked up
    public override void OnInteract(BagManager bagManager)
    {

        //Try to add item to inventory
        if (bagManager.GetInventory() != null && bagManager.GetInventory().AddItem(itemType)){
            //If succesful, destroy this object
            Destroy(gameObject);
        }
    }
}
