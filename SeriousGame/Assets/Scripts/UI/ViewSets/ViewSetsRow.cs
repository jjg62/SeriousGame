using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//One row in the ViewSets menu, displaying contents of a bag
public class ViewSetsRow : MonoBehaviour
{
    //ID of bag being displayed: This needs to be passed when obj is instantiated
    public int invID;

    public Inventory inv; //Inventory with that ID
    public BagManager bagManager; //Reference to player's bag manager
    public Podium onPodium; //If the bag is on a podium, which one

    [SerializeField]
    private GameObject uiSlotPref;

    private List<GameObject> slots;

    private char iconChar;

    //Start is called before the first frame update
    //But after invID has been passed with instantiation
    void Start()
    {
        bagManager = GameObject.FindWithTag("Player").GetComponent<BagManager>();

        if(invID == -1)
        {
            //-1 is a special input, representing the Universal set in this level
            inv = GameObject.FindWithTag("Universe").GetComponent<Universe>().GetInventory();
            iconChar = 'U';
        }
        else
        {
            //Get inventory of the bag with this ID
            inv = bagManager.GetInventoryOf(invID);
            iconChar = 'b';

            //Look through all bagpickups, see if one with same id is on a podium
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("BagPickup"))
            {
                BagPickup bag = o.GetComponent<BagPickup>();
                if (bag.id == invID)
                {
                    onPodium = bag.onPodium;
                    break;
                }
            }

        }

        slots = new List<GameObject>();

        //Create the icons that are displayed on this row
        InitSlots();
    }

    //Curly brackets should take half the width
    private List<char> halfWidthChars = new List<char> { '{', '}' };

    private float currentX;
    //Create an image at currentX, and move currentX
    private RawImage CreateIcon(char iconChar)
    {
        float w = uiSlotPref.GetComponent<RectTransform>().rect.width;

        //Move currentX by half as much if icon is a curly bracket
        currentX += halfWidthChars.Contains(iconChar) ? w / 4 : w / 2;

        //Create the slot, set its texture
        RawImage newSlot = Instantiate(uiSlotPref, transform).GetComponent<RawImage>();
        newSlot.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, 2);
        newSlot.texture = Icons.icons[iconChar].texture;

        currentX += halfWidthChars.Contains(iconChar) ? w / 4 : w / 2;

        return newSlot;
    }

    private void InitSlots()
    {
        //Clear old slots
        foreach(GameObject o in slots)
        {
            Destroy(o);
        }
        slots.Clear();

        //Calculate width needed to fit all icons on this row
        float w = uiSlotPref.GetComponent<RectTransform>().rect.width;
        float totalWidth = (inv.GetCardinality() + 3 + (onPodium != null ? 2 : 0)) * w;
        //(size of inventory, 1 for bag, 1 for =, 0.5 for each {}. If on podium, need 1 for podium icon, 1 for equal)

        currentX = -totalWidth/2.0f;

        //Podium Icon
        if(onPodium != null)
        {
            RawImage podiumSlot = CreateIcon('p');
            podiumSlot.color = onPodium.GetComponent<SpriteRenderer>().color;


            //Equals icon
            CreateIcon('=');
        }

        //Bag Icon
        RawImage bagSlot = CreateIcon(iconChar);
        bagSlot.color = inv.colour;

        //First, generate string containing a character for each item in the set
        string iconsToDraw = "={";
        foreach(Item i in inv.GetItems())
        {
            iconsToDraw += (char)(48 + (int)i.type);
        }
        iconsToDraw += "}";

        
        foreach(char c in iconsToDraw)
        {
            CreateIcon(c);
        }

    }
}
