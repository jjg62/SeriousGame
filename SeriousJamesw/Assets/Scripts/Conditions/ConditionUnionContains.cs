using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Union of two sets is a superset of a given set
public class ConditionUnionContains : Condition
{
    //Containers of two inventories
    [SerializeField]
    private IInventoryContainer invContainer1;
    [SerializeField]
    private IInventoryContainer invContainer2;

    //Required subset
    [SerializeField]
    private List<Item.Type> items;

    public override string GetCondString()
    {
        //Create string of icons, one for each item in the set
        string itemChars = StringFromSet(items);

        return "{" + itemChars + "}s" + invContainer1.GetIcon() + "u" + invContainer2.GetIcon();
    }

    public override bool Test()
    {
        Inventory inv1 = invContainer1.GetInventory();
        Inventory inv2 = invContainer2.GetInventory();
        //Get two inventories, if one can't be retrieved, fail
        if ((inv1 == null) || (inv2 == null)) return false;

        foreach (Item.Type i in items)
        {
            //If neither inventory contains this item in the subset, fail
            if (inv1.Contains(i) <= 0 && inv2.Contains(i) <= 0) return false;
        }

        return true;
    }
}
