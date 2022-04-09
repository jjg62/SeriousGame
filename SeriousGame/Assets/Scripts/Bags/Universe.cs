using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to store the universal set for a level
public class Universe : IInventoryContainer
{
    //Define the set in inspector
    [SerializeField]
    private List<Item.Type> set;

    //Create an inventory component holding the set
    private Inventory inv;
    private void Start()
    {
        inv = gameObject.AddComponent<Inventory>();
        inv.colour = Color.white;
        inv.SetItems(set);
        inv.cantDrop = true;
    }

    //Interface to get the inventory
    public override Inventory GetInventory()
    {
        return inv;
    }

    //Icon when displayed in conditions
    public override string GetIcon()
    {
        return "U";
    }
}
