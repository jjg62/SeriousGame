using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to player object
//Used to swap currently held bag
public class BagManager : IInventoryContainer
{
    //Earlier levels shouldn't allow bags to be dropped, enable this for levels with multiple bags
    public bool multipleBagsAllowed = false;

    //Keep a list of inventories, each holding different items, accesible using associated bag ID
    private Dictionary<int, Inventory> bags;

    //ID of bag currently held by player
    private int currentInventory;

    //Keeps track of when player is in a red zone to prevent them from breaking condition
    [HideInInspector]
    public RedZone inRedZone;

    //Prefab of bag, created in world when a bag is dropped
    [SerializeField]
    private BagPickup bagPref;

    //Bag sprite that the player carries
    [SerializeField]
    private SpriteRenderer bagSprite;

    //When object is created, before start
    private void Awake()
    {
        bags = new Dictionary<int, Inventory>();
        bags.Add(0, GetComponent<Inventory>()); //Add starting inventory to dictionary
    }

    //Before first frame
    private void Start()
    {
        //Set starting inventory
        SetCurrentInventory(0);
    }

    //Every frame
    private void Update()
    {
        //Drop bag if button is pressed
        if (multipleBagsAllowed && Input.GetKeyDown(KeyCode.Q))
        {
            DropBag();
        }
    }

    //Create new inventory when player picks up a new bag
    public void AddNewInventory(int id, Color col)
    {
        Inventory newInv = gameObject.AddComponent<Inventory>(); //Add another inventory component to player
        newInv.colour = col;
        bags.Add(id, newInv); //Add it to the bags dictionary
        SetCurrentInventory(id); //Becomes new held bag
    }

    //Change the player's currently held bag
    //-1 can be used as argument to set no inventory
    private void SetCurrentInventory(int id)
    {
        //Disable all inventories
        foreach (KeyValuePair<int, Inventory> kvp in bags)
        {
            kvp.Value.enabled = false;
        }

        currentInventory = id;

        if (id != -1)
        {
            //Enable the one with the right id
            bags[id].enabled = true;

            //Update HUD
            HUD.instance.inventory.UpdateInventoryUI(bags[currentInventory]);

            //Update player's bag sprite
            bagSprite.enabled = true;
            bagSprite.color = bags[id].colour;
        }
        else
        {
            //No bag held
            bagSprite.enabled = false;
            HUD.instance.inventory.UpdateInventoryUI(null); //Get rid of inventory HUD
        }

        //Updates conditions of green and red zones according to new inventory
        UpdateConditions();
    }

    //When a bag is picked up in the world
    public void PickupBag(int id, Color col)
    {
        //Drop current bag
        DropBag();

        //Swap inventory if this bag has already been picked up, otherwise create a new inventory first
        if (bags.ContainsKey(id))
        {
            SetCurrentInventory(id);
        }
        else
        {
            AddNewInventory(id, col);
        }

    }

    //Drop currently held bag into the world
    private void DropBag()
    {
        //If currently holding a bag and not in a red zone
        if(currentInventory > -1 && inRedZone == null)
        {
            //Create new bag drop in the world, and set its properties to currently held bag
            BagPickup oldBag = Instantiate(bagPref);
            oldBag.transform.position = transform.position;
            oldBag.colour = bags[currentInventory].colour;
            oldBag.id = currentInventory;

            //Player no longer holding a bag
            SetCurrentInventory(-1);
        }
        else if(inRedZone != null)
        {
            //Alert player that they can't drop bag in a red zone
            inRedZone.display.Flash();
            AudioManager.instance.Play("Fail");
        }
    }

    //Interface to get currently held inventory, inhertited from IInventoryContainer
    public override Inventory GetInventory()
    {
        if(currentInventory == -1)
        {
            return null;
        }
        else
        {
            return bags[currentInventory];
        } 
    }

    //In conditions, this InventoryContainer's icon is a bag
    public override string GetIcon()
    {
        //Get colour of inventory, set to grey if player is not holding a bag
        Inventory inv = GetInventory();
        Color col;

        if (inv == null) col = Color.grey;
        else col = inv.colour;

        return "b@" + ColorUtility.ToHtmlStringRGB(col) + "@";
    }

    //Get the inventory of the bag with some id
    public Inventory GetInventoryOf(int id)
    {
        return bags[id];
    }

    //Get list of ids of bags that have been picked up
    public List<int> GetKeys()
    {
        return new List<int>(bags.Keys);
    }
}
