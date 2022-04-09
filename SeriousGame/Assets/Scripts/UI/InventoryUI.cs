using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The HUD UI displaying items in player's current inventory
public class InventoryUI : MonoBehaviour
{
    //Back panel
    [SerializeField]
    private RawImage back;

    //Bag image
    [SerializeField]
    private RawImage invIcon;
    private Vector2 defaultInvIconSize;

    //Icon of Q key for dropping bag
    [SerializeField]
    private RawImage bagKeyIcon;

    //Array of slots in the inventory, assigned in inspector
    [SerializeField]
    private InventorySlot[] slots;

    //Reference to player's bag manager
    public BagManager playerBagManager;
    private void Start()
    {
        playerBagManager = GameObject.FindWithTag("Player").GetComponent<BagManager>();
        defaultInvIconSize = invIcon.transform.localScale;
    }

    //Get items in inventory and display them on the HUD
    public void UpdateInventoryUI(Inventory inv)
    {
        if (inv != null)
        {
            gameObject.SetActive(true);
            List<Item> items = inv.GetItems();
            //For each item in the inventory
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < items.Count)
                {
                    //Display item type and quantity
                    slots[i].Set(Icons.icons[(char)(48 + (int)items[i].type)], items[i].quantity);
                }
                else
                {
                    //No items in these slots
                    slots[i].Clear();
                }
            }
            back.color = inv.colour;
            invIcon.color = inv.colour;
            bagKeyIcon.color = inv.colour;
        }
        else
        {
            //If player holds no bag, don't show the HUD
            gameObject.SetActive(false);
        }
        
    }

    //Called when user clicks on inventory slots
    public void DropItemFromSlot(int slot)
    {
        if (playerBagManager != null)
        {
            playerBagManager.GetInventory().DropItem(slot);
        }
    }


    //Animations
    //Pulse size of all items in inventory
    public void Pulse(float size, float duration)
    {
        foreach(InventorySlot slot in slots)
        {
            slot.Pulse(size, duration);
        }
    }

    //Pulse size of a specific slot
    public void PulseSlot(int slot, float size, float duration)
    {
        slots[slot].Pulse(size, duration);
    }

    //Pulse bag image
    private IEnumerator bagPulseCoroutine;
    public void PulseBag()
    {
        //Stop the coroutine if already in progress
        if (bagPulseCoroutine != null) StopCoroutine(bagPulseCoroutine);
        bagPulseCoroutine = PulseBag(1.6f, 0.4f);
        StartCoroutine(bagPulseCoroutine);
    }

    IEnumerator PulseBag(float size, float duration)
    {
        Vector2 newScale = new Vector2(size, size);

        //Instantly set scale to target
        invIcon.transform.localScale = newScale;

        //Then gradually decrease back to normal scale
        float t = 0;
        while (t < duration)
        {
            invIcon.transform.localScale = Vector2.Lerp(newScale, defaultInvIconSize, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        invIcon.transform.localScale = defaultInvIconSize;
    }

    //Gradually change colour of the inventory
    public void FadeColour(Color c, float duration)
    {
        StartCoroutine(FadeColourAnimation(c, duration));
    }

    IEnumerator FadeColourAnimation(Color c, float duration)
    {
        Color oldColour = back.color;
        float t = 0;
        //Linearly interpolate between current colour and desired colour
        while(t < duration)
        {
            back.color = Color.Lerp(oldColour, c, t / duration);

            t += Time.deltaTime;
            yield return null;
        }
        back.color = c;
    }
}
