using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inventory must not contain an element
public class ConditionNEOf : Condition
{
    //The type of item that must not be in the inventory
    [SerializeField]
    private Item.Type item;

    //Container being checked (player, podium)
    [SerializeField]
    private IInventoryContainer invContainer;

    public override bool Test()
    {
        return invContainer.GetInventory() != null && invContainer.GetInventory().Contains(item) == 0;
    }

    public override string GetCondString()
    {
        char itemChar = (char)(48 + (int)item);
        return itemChar + "n" + invContainer.GetIcon();
    }
}
