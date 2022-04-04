using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains items, their quantity, and functionality for adding/removing items
public class Inventory : MonoBehaviour
{
    //Currently held items
    private List<Item> items;

    //Reference to BagManager
    private BagManager bags;

    //Only player can drop items - other objects have inventories but can't drop (podiums, universe set)
    [HideInInspector]
    public bool cantDrop = false;

    //The colour of this inventory in HUD and as a BagPickup
    public Color colour;

    private void Awake()
    {
        //Instantiate items list
        items = new List<Item>();      
    }

    //Before first frame
    private void Start()
    {
        bags = GetComponent<BagManager>();   
        UpdateConditions();
    }

    //Every frame
    private void Update()
    {
        if (!cantDrop)
        {
            //Num keys to drop items
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DropItem(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DropItem(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                DropItem(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                DropItem(3);
            }
        }
    }

    //Update status of red/green zones
    private void UpdateConditions()
    {
        //Find all objects that update when an inventory changes
        GameObject[] condObjs = GameObject.FindGameObjectsWithTag("InventoryCheck");
        foreach(GameObject o in condObjs)
        {
            o.GetComponent<IInventoryCheck>().InventoryCheck(this);
        }
    }

    //Add an item to this inventory
    public bool AddItem(Item.Type type)
    {
        //Is the player in a red zone and currently passing its condition?
        bool passedBefore = bags.inRedZone != null && bags.inRedZone.con.Test();

        bool added = false;
        //First check if item of same type is already in inventory
        foreach(Item item in items)
        {
            if(item.type == type)
            {
                //item.quantity++; //Un-comment this for multi-set behaviour, no longer used in this build but may be useful again later
                added = true;
                break;
            }
        }

        //Otherwise, need a whole new instance
        if (!added)
        {
            Item newItem = new Item(type);
            items.Add(newItem);

            //If fail red zone now after adding the item, undo the change
            if (passedBefore && !bags.inRedZone.con.Test())
            {
                items.Remove(newItem);
                bags.inRedZone.display.Flash();
                AudioManager.instance.Play("Fail");
                return false;
            }
        }

        //Update HUD
        HUD.instance.inventory.UpdateInventoryUI(this);
        HUD.instance.viewSets.Refresh();

        //Update conditions based on inventory
        UpdateConditions();

        return true;
    }

    //Drop an item from a position in the inventory
    public void DropItem(int pos)
    {
        //Don't index out of list, or while travelling through a gate
        if (pos >= items.Count || pos < 0) return;

        //Get item type
        Item.Type droppedType = items[pos].type;

        //Can't drop 'cursed' items - id 4,5,6,7
        if ((int)droppedType > 3)
        {
            HUD.instance.inventory.PulseSlot(pos, 1.6f, 0.4f); //Make item slot with cursed item pulse
            AudioManager.instance.Play("Fail");
            return;
        }

        //Is the player in a red zone and currently passing its condition?
        bool passedBefore = bags.inRedZone != null && bags.inRedZone.con.Test();

        //Decrement Quantity
        items[pos].quantity--;

        //Check that the update will not break current condition
        if (passedBefore && !bags.inRedZone.con.Test())
        {
            //Undo the change
            bags.inRedZone.display.Flash();
            AudioManager.instance.Play("Fail");
            items[pos].quantity++;
            return;
        }

        //If last one, remove
        if (items[pos].quantity <= 0)
        {
            items.RemoveAt(pos);
        }

        //Spawn a pickup
        GameObject spawned = Instantiate(Items.items[(int)droppedType].gameObject);
        spawned.transform.position = transform.position;

        //Update HUD
        HUD.instance.inventory.UpdateInventoryUI(this);
        HUD.instance.viewSets.Refresh();

        //Update conditions based on inventory
        UpdateConditions();

        AudioManager.instance.Play("Drop");
    }

    //Does this inventory contain a certain type of item? Returns quantity if so
    public int Contains(Item.Type type)
    {
        foreach(Item item in items)
        {
            if(item.type == type)
            {
                return item.quantity;
            }
        }
        return 0;
    }

    //Is this inventory a subset of some given set?
    public bool SubsetOf(List<Item.Type> super)
    {
        foreach(Item i in items)
        {
            if (!super.Contains(i.type)) return false;
        }
        //Otherwise, the superset contains everything in this inventory
        return true;
    }

    //Is this inventory a superset of some given set?
    public bool SupersetOf(List<Item.Type> sub)
    {
        foreach(Item.Type i in sub)
        {
            if (Contains(i) == 0) return false;
        }

        //Otherwise, this inventory contains everything in sub
        return true;
    }

    //Get cardinality of this inventory
    public int GetCardinality()
    {
        int total = 0;
        foreach(Item i in items)
        {
            if (i.quantity > 0) total++;
        }
        //Return number of items with non-zero quantity
        return total;
    }

    //Get inventory as a list of items
    public List<Item> GetItems()
    {
        return items;
    }

    //Set inventory contents directly
    public void SetItems(List<Item.Type> i)
    {
        items.Clear();
        foreach(Item.Type t in i)
        {
            items.Add(new Item(t));
        }
    }
}
