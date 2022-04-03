using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Union should be equal to a given set
public class ConditionUnion : Condition
{
    //Containers of two sets
    [SerializeField]
    private IInventoryContainer invContainer1;
    [SerializeField]
    private IInventoryContainer invContainer2;

    //The set that the union must be equal to
    [SerializeField]
    private List<Item.Type> items;

    public override string GetCondString()
    {
        //For string of icons, one for each item in the set
        string itemChars = StringFromSet(items);

        return invContainer1.GetIcon() + "u" + invContainer2.GetIcon() + "={" + itemChars + "}";
    }

    public override bool Test()
    {
        Inventory inv1 = invContainer1.GetInventory();
        Inventory inv2 = invContainer2.GetInventory();
        //Get inventories from each container, if one can't be retrieved, fail
        if ((inv1 == null) || (inv2 == null)) return false;


        //Find the union of both inventories
        List<Item> union = new List<Item>();
        foreach (Item i in inv1.GetItems())
        {
            if (i.quantity > 0)
            {
                union.Add(i);
                //If the required set doesn't contain this item, the union won't be equal to it
                if (!items.Contains(i.type)) return false;
            }
        }

        foreach (Item i in inv2.GetItems())
        {
            if (i.quantity > 0)
            {
                if (inv1.Contains(i.type) > 0) continue; //Don't add the item if it's already there
                union.Add(i);
                //If the required set doesn't contain this item, the union won't be equal to it
                if (!items.Contains(i.type)) return false;
            }
        }


        //At this point we have union, and we know that it's a subset of items
        //So can just check size
        return union.Count == items.Count;
    }
}
