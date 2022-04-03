using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//List of item prefabs for the Game gameobject
public class Items : MonoBehaviour
{
    public List<ItemPickup> itemList; //Prefabs passed in inspector
    public static List<ItemPickup> items; 

    //When object is created
    private void Awake()
    {
        //Create a static copy
        items = new List<ItemPickup>();
        foreach (ItemPickup i in itemList)
        {
            items.Add(i);
        }
    }
}
