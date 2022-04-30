using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for items stored in an inventory
public class Item
{
    //Possible item types
    public enum Type
    {
        Triangle = 0,
        Square = 1,
        Circle = 2,
        Diamond = 3,
        Club = 4,
        Heart = 5,
        Spade = 6,
        CDiamond = 7
    }

    public Item(Type type)
    {
        this.type = type;
        this.quantity = 1;
    }

    public Type type; //Type of this item

    //Quantity was used in an older build of the game, now may only have 0 or 1
    //But, might be useful later for a multiset mechanic
    public int quantity;

}
