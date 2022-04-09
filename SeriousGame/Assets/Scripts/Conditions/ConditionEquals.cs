using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inventory must be equal to a given set
public class ConditionEquals : Condition
{
    //The required set
    [SerializeField]
    private List<Item.Type> items;

    //Container being checked
    [SerializeField]
    private IInventoryContainer invContainer;

    public override string GetCondString()
    {
        //Create string of icons, one for each item the set
        string itemChars = StringFromSet(items);

        return "b={" + itemChars + "}";
    }

    public override bool Test()
    {
        Inventory inv = invContainer.GetInventory();
        return inv != null && inv.GetCardinality() == items.Count && inv.SubsetOf(items);
    }
}
