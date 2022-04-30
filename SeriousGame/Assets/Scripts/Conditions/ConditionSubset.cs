using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inventory must be a subset of a given set
public class ConditionSubset : Condition
{
    //Required superset
    [SerializeField]
    private List<Item.Type> items;

    //Container being checked (player, podium)
    [SerializeField]
    private IInventoryContainer invContainer;

    public override bool Test()
    {
        return invContainer.GetInventory() != null && invContainer.GetInventory().SubsetOf(items);
    }

    public override string GetCondString()
    {
        //Create string of icons, one for each item in the set
        string itemChars = StringFromSet(items);
        return invContainer.GetIcon() + "s{"+itemChars+"}";
    }
}
