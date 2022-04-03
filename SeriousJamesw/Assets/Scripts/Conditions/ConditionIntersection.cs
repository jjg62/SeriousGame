using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Intersection of two inventories must be equal to a given set
public class ConditionIntersection : Condition
{
    //The two containers whose intersection is being checked
    [SerializeField]
    private IInventoryContainer invContainer1;
    [SerializeField]
    private IInventoryContainer invContainer2;

    //The given set it must be equal to
    [SerializeField]
    private List<Item.Type> items;

    public override string GetCondString()
    {
        //Create string of icons, one for each item in the set
        string itemChars = StringFromSet(items);

        return invContainer1.GetIcon() + "i" + invContainer2.GetIcon() + "={" + itemChars + "}";
    }

    public override bool Test()
    {
        Inventory inv1 = invContainer1.GetInventory();
        Inventory inv2 = invContainer2.GetInventory();
        //Get two inventories, if one can't be retrieved, fail
        if ((inv1 == null) || (inv2 == null)) return false;


        //Create the intersection
        List<Item> intersection = new List<Item>();
        foreach(Item i in inv2.GetItems())
        {
            if (i.quantity <= 0) continue; //Check the quantity is not zero
            foreach(Item j in inv1.GetItems())
            {
                if (j.quantity <= 0) continue; //Same in the other inventory

                //If both contain the same type, add it to the intersection
                if (i.type == j.type){
                    intersection.Add(i);
                    //Check that the required set also contains this item
                    if (!items.Contains(i.type)) return false;
                    break;
                }
            }
        }
        //At this point we have intersection, and we know that it's a subset of items
        //So can just check size
        return intersection.Count == items.Count;
    }
}
