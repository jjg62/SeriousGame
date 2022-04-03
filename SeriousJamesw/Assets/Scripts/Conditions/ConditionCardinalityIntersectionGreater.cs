using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cardinality of intersection must be greater than some value
public class ConditionCardinalityIntersectionGreater : Condition
{
    //Two containers whose intersection is being checked
    [SerializeField]
    private IInventoryContainer invContainer1;
    [SerializeField]
    private IInventoryContainer invContainer2;

    //Minimum required size
    [SerializeField]
    private int size;

    public override string GetCondString()
    { 
        return "|" + invContainer1.GetIcon() + "i" + invContainer2.GetIcon() + "|/>//" + size + "/";
    }

    public override bool Test()
    {
        Inventory inv1 = invContainer1.GetInventory();
        Inventory inv2 = invContainer2.GetInventory();
        //Get two inventories, if one can't be retrieved, fail
        if ((inv1 == null) || (inv2 == null)) return false;


        //Create intersection of both inventories
        List<Item> intersection = new List<Item>();
        foreach (Item i in inv2.GetItems())
        {
            if (i.quantity <= 0) continue; //Ensure quantity is not 0
            foreach (Item j in inv1.GetItems())
            {
                if (j.quantity <= 0) continue; //Same for other inventory

                //If they both contain an item of the same type, add it to the intersection
                if (i.type == j.type)
                {
                    intersection.Add(i);
                    break;
                }
            }
        }

        return intersection.Count > size;
    }
}
