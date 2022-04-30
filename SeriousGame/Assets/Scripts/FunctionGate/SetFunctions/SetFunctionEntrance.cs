using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checks for when player is nearby with an empty inventory, ready to vault
public class SetFunctionEntrance : MonoBehaviour, IInventoryCheck
{
    //Reference to gate
    [SerializeField]
    private SetFunctionGate funcGate;

    private bool inRange; //Is the player in the trigger
    private bool noBag; //Does the player have no bag

    //When inventory updated
    public void InventoryCheck(Inventory inv)
    {
        //If player not holding bag
        if (inv == null)
        {
            noBag = true;
            if (inRange) funcGate.TryVault(); //If player is already in range, vault
        }
        else
        {
            noBag = false;
        }
    }

    //When object enters the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //If it's the player, they're in range
            inRange = true;
            if (noBag)
            {
                funcGate.TryVault(); //If they have no bag, vault
            }
            else if(funcGate.IsWorking())
            {
                //If they have a bag but they would be able to vault, alert them of bag
                HUD.instance.inventory.PulseBag();
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
