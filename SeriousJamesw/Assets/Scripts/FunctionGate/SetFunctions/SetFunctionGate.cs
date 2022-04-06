using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

//Function Gate for Bags/Sets 
public class SetFunctionGate : HasDisplay
{
    //Normal direction or inverse
    [SerializeField]
    private bool isInverse;

    //The inverse of this gate
    [SerializeField]
    private SetFunctionGate inverse;

    //The function to apply to bags
    [SerializeField]
    private SetFunction setFunc;

    //Exit position of vault
    [SerializeField]
    private Transform target;

    //Collider blocking passage
    [SerializeField]
    private Collider2D col;

    //Is player in range of vault
    private bool playerInRange;

    //References to player
    private BagManager playerBags;
    private PlayerInteract playerInteract;

    //Particle effect for vault
    [SerializeField]
    private ParticleSystem vaultEffectPref;

    //Animator for the gate
    private Animator anim;

    //Post processing
    [SerializeField]
    private Volume vol;

    //Sprites
    private SpriteRenderer gateSprite;
    private SpriteRenderer effectSprite;

    //Bag prefab to create
    [SerializeField]
    private BagPickup bagPref;

    //Particle System for absorb effect
    [SerializeField]
    private ParticleSystem absorbEffect;

    //Vault Indicator Prefab
    [SerializeField]
    private SpriteRenderer vaultIndicator;

    //Before first frame
    private new void Start()
    {    
        //Get reference
        playerBags = GameObject.FindWithTag("Player").GetComponent<BagManager>();
        playerInteract = playerBags.gameObject.GetComponent<PlayerInteract>();

        anim = GetComponent<Animator>();

        gateSprite = GetComponent<SpriteRenderer>();
        effectSprite = GetComponentInChildren<SpriteRenderer>();

        //Absorb effect should be off by default
        absorbEffect.Stop();

        //Create vault indicator from prefab
        vaultIndicator = Instantiate(vaultIndicator);
        vaultIndicator.transform.position = transform.position;
        vaultIndicator.enabled = false; //Off by default

        if (!isInverse)
        {
            //Use normal condition display for set function gates
            base.Start();
            display.SetCondition(setFunc.GetDisplayString());
            anim.SetBool("Bijective", setFunc.isBijective());
        }

    }

    //When object enters trigger
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (setFunc.CanApply() && !isInverse || (isInverse && setFunc.isBijective()))
        {
            //If function/inverse is well defined:

            if (collision.tag == "Player")
            {
                //If it was the player, they are now in range
                playerInRange = true;
                absorbEffect.Play();
                vaultIndicator.enabled = true;
            }

            BagPickup bag = collision.gameObject.GetComponent<BagPickup>();

            //If the object that entered is a bag drop, and was not just launched by this gate
            if (bag != null && bag.launchedBy == null)
            {
                bag.DisableInteract(); //Stop item from being able to be picked up
                bag.launchedBy = gameObject;
                StartCoroutine(transformBag(bag));
            }
        }
    }

    //When object exits trigger
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false; //If it was the player, they are now out of range
            absorbEffect.Stop();
            vaultIndicator.enabled = false;
        }
    }


    //Every frame
    private new void Update()
    {
        //If not holding bag, press f near gate, and not already travelling
        if (playerInRange && col.enabled && Input.GetKeyDown(KeyCode.F) && (!isInverse || setFunc.isBijective()))
        {
            if (playerBags.GetInventory() == null)
            {
                //Allow passage
                col.enabled = false;

                //Disable picking up
                playerInteract.enabled = false;

                //Start vault animation
                StartCoroutine(playerVault());
            }
            else
            {
                AudioManager.instance.Play("Fail");
                HUD.instance.inventory.PulseBag();
            }

        }
        if(!isInverse) base.Update();
    }

    //Transform bag animation
    private IEnumerator transformBag(BagPickup bag)
    {
        //Get inventory of this bag
        int id = bag.id;
        Inventory inv = playerBags.GetInventoryOf(bag.id);
        Color col = bag.colour;

        AudioManager.instance.Play("FunctionCharge");

        //Absorb the bag into the gate
        float t = 0;
        float absorbDuration = 0.3f;
        Vector2 originalPos = bag.transform.position;

        //Linearly interpolate position over time
        while (t < absorbDuration)
        {
            bag.transform.position = Vector2.Lerp(originalPos, transform.position, t / absorbDuration);
            t += Time.deltaTime;
            yield return null;
        }

        //Delete bag
        Destroy(bag.gameObject);

        yield return new WaitForSeconds(0.2f);

        AudioManager.instance.Play("FunctionTransform");

        //Change the inventory matching this bag
        inv.SetItems(isInverse ? setFunc.ApplyInverse(inv.GetItems()) : setFunc.Apply(inv.GetItems()));
        BagPickup newBag = Instantiate(bagPref);

        //Create new bag drop for the same inventory, launch it
        newBag.transform.position = transform.position;
        newBag.id = id;
        newBag.colour = col;
        newBag.launchedBy = gameObject;
        newBag.launchAngle = setFunc.transform.localRotation.eulerAngles.z * Mathf.Deg2Rad + (isInverse ? Mathf.PI : 0);

    }

    //Player vault animation
    private IEnumerator playerVault()
    {
        Vector2 originalPos = playerBags.transform.position;
        float t = 0;
        float vaultDuration = 0.3f;

        //Vault particle effect
        Instantiate(vaultEffectPref, playerBags.transform).transform.position = playerBags.transform.position;
        AudioManager.instance.Play("FunctionDash");

        //Move player over function gate
        while (t < vaultDuration)
        {
            playerBags.transform.position = Vector2.Lerp(originalPos, target.transform.position, t / vaultDuration);
            vol.weight = Mathf.Min(1 - t / vaultDuration, t / vaultDuration);

            t += Time.deltaTime;
            yield return null;
        }

        //Re-enable collider and picking up
        col.enabled = true;
        playerInteract.enabled = true;
    }
}
