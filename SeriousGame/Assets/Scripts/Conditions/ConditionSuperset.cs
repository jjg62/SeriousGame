using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inventory must be a superset of a given set
public class ConditionSuperset : Condition
{
    //Container with the inventory being checked (player, podium)
    [SerializeField]
    private IInventoryContainer invContainer;

    //Required subset
    [SerializeField]
    private List<Item.Type> items;

    public override bool Test()
    {
        return invContainer.GetInventory() != null && invContainer.GetInventory().SupersetOf(items);
    }

    public override string GetCondString()
    {
        //Create string of icons, one for each item in the set
        string itemChars = StringFromSet(items);
        return "{" + itemChars + "}sb";
    }
}
