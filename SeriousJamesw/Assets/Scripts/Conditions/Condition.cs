using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A Condition subclass to be attached to Red Zones and Green Zones
//Use inspector to specify details about the condition
public abstract class Condition : MonoBehaviour
{
    //Get reference to player
    protected GameObject player;

    protected void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    //Helper function - get a string of icons, one fore each item in a set
    protected string StringFromSet(List<Item.Type> items)
    {
        string itemChars = "";
        foreach (Item.Type i in items)
        {
            itemChars += (char)(48 + (int)i);
        }
        return itemChars;
    }

    public abstract bool Test(); //Return true if player passes condition, false if not
    public abstract string GetCondString(); //Get a string of icons to display describing the condition
}
