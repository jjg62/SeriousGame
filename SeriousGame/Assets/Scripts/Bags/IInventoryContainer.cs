using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class for objects that can have an inventory
//Used for player, podiums and the universal set
public abstract class IInventoryContainer : MonoBehaviour
{
    public abstract Inventory GetInventory();
    public abstract string GetIcon(); //Icon to use in conditions

    //Update RedZone/GreenZone conditions (they should be tagged with InventoryCheck)
    public void UpdateConditions()
    {
        GameObject[] condObjs = GameObject.FindGameObjectsWithTag("InventoryCheck");
        foreach (GameObject o in condObjs)
        {
            o.GetComponent<IInventoryCheck>().InventoryCheck(GetInventory());
        }
    }
}
