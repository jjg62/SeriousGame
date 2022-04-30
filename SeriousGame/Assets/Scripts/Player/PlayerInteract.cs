using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps track of which objects are in range of interaction / picking up
public class PlayerInteract : MonoBehaviour
{
    //Interactables in range
    private List<Interactable> inRange;

    //Latest interactable to come into range
    private Interactable latest;
    
    //Reference to bag manager
    private BagManager bagManager;

    //Indicator showing which interactable is selected
    [SerializeField]
    private PickupIndicator indicatorPref; //Prefab
    private PickupIndicator indicator;

    //Before first frame
    private void Start()
    {
        //Get reference
        bagManager = GetComponent<BagManager>();

        inRange = new List<Interactable>(); 
        //Instantiate indicator object
        indicator = Instantiate(indicatorPref);
    }

    //Every frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) Interact();
    }

    //When interactable comes into range, add it to list
    public void AddInRange(Interactable i)
    {
        inRange.Add(i);

        bool hasBag = bagManager.GetInventory() != null;
        if (!(i is ItemPickup) || hasBag) latest = i; //It becomes the latest

        MoveIndicator(); //Indicator moves to this item
    }

    //When player leaves interact range
    public void OutOfRange(Interactable i)
    {
        if (inRange.Contains(i)) inRange.Remove(i); //Remove from the list
        if (latest == i)
        {
            //If it was the latest, select a new pickup
            latest = FindNewLatest();

            //Move indicator to new latest's position
            MoveIndicator();
        }
       
    }

    //Find the new interactable to move the indicator to, dependent on whether player has bag
    private Interactable FindNewLatest()
    {

        bool hasBag = bagManager.GetInventory() != null;
        foreach(Interactable i in inRange)
        {
            //Find first item that player can interact with (items only allowed if player has a bag)
            if (hasBag || !(i is ItemPickup)) return i;
        }
        return null;
    }

    //Call OnInteract on selected interactable
    public void Interact()
    {
        if(latest != null)
        {
            indicator.transform.SetParent(null);
            latest.OnInteract(bagManager);
        }
    }

    //Move indicator to selected interactable
    private void MoveIndicator()
    {
        if(latest != null)
        {
            indicator.Activate(true);
            indicator.transform.position = latest.transform.position;
            indicator.transform.SetParent(latest.transform); //Set parent, such that indicator moves with interactable
        }
        else
        {
            indicator.transform.SetParent(null);
            indicator.Activate(false);
        }

    }
}
