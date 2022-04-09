using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to bag objects that can be picked up in the world
public class BagPickup : Pickup
{
    public Color colour; //Colour of bag sprite and HUD
    public int id; //ID used by BagManager

    //Store the podium this bag is on
    [HideInInspector]
    public Podium onPodium;

    //Before first frame
    private new void Start()
    {
        GetComponentInChildren<SpriteRenderer>().color = colour; //Set sprite colour
        base.Start();
    }

    //What happens when picked up
    public override void OnInteract(BagManager bagManager)
    {
        if (bagManager.multipleBagsAllowed)
        {
            //Only allow bag to be picked up if not in a red zone
            if(bagManager.inRedZone == null)
            {
                Destroy(gameObject);
                bagManager.PickupBag(id, colour);
            }
            else
            {
                //Alert the player if they are in a red zone
                bagManager.inRedZone.display.Flash();
                AudioManager.instance.Play("Fail");
            }
            
        }
    }

    //When object is destroyed
    private void OnDestroy()
    {
        //Ensure when bag is picked up/destroyed, podium is cleared
        if(onPodium != null && onPodium.currentStoredBag == this)
        {
            onPodium.currentStoredBag = null;
            onPodium.UpdateConditions(); //Any conditions reliant of the podium should update
            onPodium.StopAllCoroutines(); //Stop bag absorb animation
            onPodium.absorbEffect.Play(); //Restart particles
        }
        
        HUD.instance.viewSets.Refresh(); //Update the list of bags on HUD
    }
}
