using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object in the world that can hold a bag, allowing its inventory be used in conditions
public class Podium : IInventoryContainer
{
    private BagManager bags; //Reference to player BagManager
    public BagPickup currentStoredBag;  //Bag currently stored in the podium

    public ParticleSystem absorbEffect; //Particles showing that the podium can absorb a bag

    //Before first frame
    private void Start()
    {
        bags = GameObject.FindWithTag("Player").GetComponent<BagManager>();
    }

    //When an object enters trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Try to get bag component on the object
        BagPickup bag = collision.gameObject.GetComponent<BagPickup>();
        if(bag != null && currentStoredBag == null) //If object is a dropped bag, and space available
        {
            //Stop drop movement, and set opacity to full
            Color c = bag.GetComponentInChildren<SpriteRenderer>().color;
            c.a = 1;
            bag.GetComponentInChildren<SpriteRenderer>().color = c;
            bag.StopAllCoroutines();

            //Play sound effect
            AudioManager.instance.Play("BagDrop");


            //Set currently stored bag, give reference of this to the bag for OnDestroy
            absorbEffect.Stop();
            currentStoredBag = bag;
            bag.onPodium = this;

            StartCoroutine(BagPlace(bag)); //Play the bag moving animation
            UpdateConditions(); //Update conditions that may be dependent on this podium
            HUD.instance.viewSets.Refresh(); //Refresh list of all bags on UI
        }
    }

    //Animation for bag being placed in podium
    private IEnumerator BagPlace(BagPickup bag)
    {
        //Timer variable
        float t = 0;
        float bagPlaceDuration = 0.6f;

        //Get original position and scale of bag sprite
        SpriteRenderer bagSprite = bag.GetComponentInChildren<SpriteRenderer>();

        Vector2 originalBagPos = bag.transform.position;
        Vector2 originalBagScale = bagSprite.transform.localScale;

        //Bag should fit in slot on podium sprite
        Vector2 targetPos = (Vector2)transform.position + new Vector2(0f, 0.5f);
        Vector2 targetScale = new Vector2(0.666f, 0.333f); 

        
        //Linearly move bag into slot
        while(t < bagPlaceDuration)
        {
            bag.transform.position = Vector2.Lerp(originalBagPos, targetPos, t/bagPlaceDuration);
            bagSprite.transform.localScale = Vector2.Lerp(originalBagScale, targetScale, t / bagPlaceDuration);
            t += Time.deltaTime;
            yield return null;
        }

        bag.transform.position = targetPos;

    }

    //Interface to get inventory of the bag in this podium
    public override Inventory GetInventory()
    {
        if (currentStoredBag == null) return null;
        else
        {
            return bags.GetInventoryOf(currentStoredBag.id);
        }
    }

    //Get icon of podium in conditions
    public override string GetIcon()
    {
        //A podium icon, coloured to the same colour as this podium
        return "p@"+ColorUtility.ToHtmlStringRGB(GetComponent<SpriteRenderer>().color)+"@";
    }


}
