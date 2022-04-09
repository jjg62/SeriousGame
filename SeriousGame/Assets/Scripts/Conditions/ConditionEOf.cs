using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inventory must have a certain element
public class ConditionEOf : Condition
{
    //Item that must be in the inventory
    [SerializeField]
    private Item.Type item;

    //Container being checked
    [SerializeField]
    private IInventoryContainer invContainer;

    public override bool Test()
    {
        return invContainer.GetInventory() != null && invContainer.GetInventory().Contains(item) > 0;
    }

    public override string GetCondString()
    {
        char itemChar = (char)(48 + (int)item);
        return itemChar + "e" + invContainer.GetIcon();
    }
}
