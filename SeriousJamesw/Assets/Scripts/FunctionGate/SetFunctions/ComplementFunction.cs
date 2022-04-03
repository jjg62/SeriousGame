using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Function that takes a set to its complement in a 'parent' set
public class ComplementFunction : SetFunction
{
    //Container to get parent set from
    [SerializeField]
    private IInventoryContainer parent;

    //Whether this complement function gate is bijective
    [SerializeField]
    private bool bijective;

    //Helper function to check if list of items contains a specific type
    private bool ContainsType(List<Item> input, Item.Type type)
    {
        foreach(Item i in input)
        {
            if (i.quantity > 0 && i.type == type) return true;
        }
        return false;
    }

    //Result of applying this function to a set
    public override List<Item.Type> Apply(List<Item> input)
    {
        List<Item> parentItems = parent.GetInventory().GetItems();
        List<Item.Type> output = new List<Item.Type>();

        //Copy all items from parent except those in the input
        foreach(Item i in parentItems)
        {
            if(!ContainsType(input, i.type))
            {
                output.Add(i.type);
            }
        }

        return output;
    }

    //Self inverse, if it is bijective
    public override List<Item.Type> ApplyInverse(List<Item> input)
    {
        return Apply(input);
    }

    public override bool isBijective()
    {
        return bijective;
    }

    //What to display in the ConditionDisplay
    public override string GetDisplayString()
    {
        return "b~U/\\/b";
    }

    //As long as parent container has an inventory (which can itslef be empty)
    public override bool CanApply()
    {
        return parent.GetInventory() != null;
    }
}
