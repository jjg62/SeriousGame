using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checks for when player is nearby with an empty inventory, ready to vault
public class FunctionGateEntrance : MonoBehaviour, IInventoryCheck
{
    //Reference to gate
    [SerializeField]
    private NewFunctionGate funcGate;

    private bool inRange; //Is the player in the trigger
    private bool invEmpty; //Does the player have an empty inventory

    //When inventory updated
    public void InventoryCheck(Inventory inv)
    {
        //If it's empty
        if (inv != null && inv.GetCardinality() == 0)
        {
            invEmpty = true;
            if (inRange) funcGate.TryVault(); //If player is already in range, vault

        }
        else
        {
            invEmpty = false;
        }
    }

    //When object enters the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //If it's the player, they're in range
            inRange = true;
            if (invEmpty)
            {
                funcGate.TryVault(); //If they have an empty inventory, vault
            }
            else if(funcGate.IsWorking())
            {
                //If they don't but they would be able to vault, alert them of inv items
                HUD.instance.inventory.Pulse(1.6f, 0.4f);
                AudioManager.instance.Play("Fail");
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            inRange = false;
        }  
    }


}
