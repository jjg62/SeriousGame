using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionGateEntrance : MonoBehaviour, IInventoryCheck
{
    [SerializeField]
    private NewFunctionGate funcGate;

    private bool inRange;
    private bool invEmpty;

    public void InventoryCheck(Inventory inv)
    {
        if (inv.GetCardinality() == 0)
        {
            invEmpty = true;
            if (inRange) funcGate.TryVault();

        }
        else
        {
            invEmpty = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            inRange = true;
            if (invEmpty)
            {
                funcGate.TryVault();
            }
            else if(funcGate.IsWorking())
            {
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
