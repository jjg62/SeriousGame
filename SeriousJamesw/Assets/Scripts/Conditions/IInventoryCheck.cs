using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for objects that need to update when an inventory is updated
public interface IInventoryCheck
{
    void InventoryCheck(Inventory inv);
}
