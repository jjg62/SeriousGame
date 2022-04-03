using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cardinality of a complement of two sets must be greater than some value
public class ConditionCardinalityComplementGreater : Condition
{
    //First set
    [SerializeField]
    private IInventoryContainer from;

    //Second set
    [SerializeField]
    private IInventoryContainer without;

    //Required size of complement
    [SerializeField]
    private int size;

    public override string GetCondString()
    {
        return "|" + from.GetIcon() + "/\\/" + without.GetIcon() + "|/>//"  + size + "/";
    }

    //Helper function - check if a list of items contains one with a certain type
    private bool ContainsType(List<Item> input, Item.Type type)
    {
        foreach (Item i in input)
        {
            //Check the quantity is more than 0 and type matches
            if (i.quantity > 0 && i.type == type) return true;
        }
        return false;
    }

    public override bool Test()
    {
        //Get the inventories from containers
        Inventory inv1 = from.GetInventory();
        Inventory inv2 = without.GetInventory();

        //If one can't be retrieved, fail
        if ((inv1 == null) || (inv2 == null)) return false;

        //Get lists of items from inventories
        List<Item> fromItems = inv1.GetItems();
        List<Item> withoutItems = inv2.GetItems();

        //Make new list to create complement
        List<Item.Type> output = new List<Item.Type>();

        foreach (Item i in fromItems)
        {
            //Add each item in inv1 ththat isn't in inve2
            if (!ContainsType(withoutItems, i.type))
            {
                output.Add(i.type);
            }
        }

        return output.Count > size;
    }
}
