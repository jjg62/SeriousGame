using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//Function Gate: drop items within the trigger and they'll be absorbed and transformed
public class NewFunctionGate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Is this the normal direction or the inverse
    [SerializeField]
    private bool isInverse;

    //The inverse of this function
    [SerializeField]
    private NewFunctionGate inverse;

    //The relation this gate uses
    public Relation rel;

    //Position of exit for player vault
    [SerializeField]
    private Transform target;

    //Collider preventing player passage
    [SerializeField]
    private Collider2D col;

    //Is player in range to vault
    private bool playerInRange;

    //References to player
    private BagManager playerBags;
    private PlayerInteract playerInteract;

    //Visual effect for player vault
    [SerializeField]
    private ParticleSystem vaultEffectPref;

    //Reference to animator of function gate
    private Animator anim;

    //Post-processing effect volume
    [SerializeField]
    private Volume vol;

    //Sprites - one for the gate and one for the wave effect
    private SpriteRenderer gateSprite;
    private SpriteRenderer effectSprite;

    //Particle System for absorb effect
    [SerializeField]
    private ParticleSystem absorbEffect;

    //Vault Indicator Prefab
    [SerializeField]
    private SpriteRenderer vaultIndicator;


    //Function Display
    [Header("Display (don't change for inverse)")]
    [SerializeField]
    private FunctionDisplay displayPref; //Prefab
    private FunctionDisplay display;
    [SerializeField]
    private Vector2 displayOffset; //Position compared to gate
    private float alpha; //Display Alpha
    private LineRenderer line; //Line to display
    private bool mouseHover; //Is mouse hovering over gate

   

    //Before first frame
    private void Start()
    {
        //Get references
        //Find player
        playerBags = GameObject.FindWithTag("Player").GetComponent<BagManager>();
        playerInteract = playerBags.gameObject.GetComponent<PlayerInteract>();

        line = GetComponentInChildren<LineRenderer>();
        anim = GetComponent<Animator>();

        gateSprite = GetComponent<SpriteRenderer>();
        effectSprite = GetComponentInChildren<SpriteRenderer>();

        //Absorb effect should be off by default
        absorbEffect.Stop();

        //Create vault indicator from prefab
        vaultIndicator = Instantiate(vaultIndicator);
        vaultIndicator.transform.position = transform.position;
        vaultIndicator.enabled = false; //Off by default


        //Display only needs to be instantiated once, not also for inverse
        if (!isInverse)
        {
            display = Instantiate(displayPref);
            display.transform.position = transform.position + (Vector3)displayOffset;
            UpdateDisplay();
            line.SetPosition(0, transform.position); //Line starts at position of gate
        }
        
    }

    //When object enters trigger
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((!isInverse && rel.isFunction()) || (isInverse && rel.isBijective()) )
        {
            //If function/inverse is well-defined:


            if (collision.tag == "Player") //If it's the player, they're in range for vault
            {
                playerInRange = true;
                absorbEffect.Play();
                vaultIndicator.enabled = true;
            }

            ItemPickup item = collision.gameObject.GetComponent<ItemPickup>();
            //If the object that entered is an item drop, and was not just launched by this gate
            if (item != null && item.launchedBy == null)
            {
                item.DisableInteract(); //Stop item from being able to be picked up
                StartCoroutine(transformItem(item)); //Start the transform animation
            }
        }
    }

    //When object exits trigger
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false; //If it's the player, they're out of range for vault
            absorbEffect.Stop();
            vaultIndicator.enabled = false;
        }
    }

    //Update the function display
    public void UpdateDisplay()
    {
        display.SetFunction(rel);
        anim.SetBool("Bijective", rel.isBijective());
        line.SetPosition(1, display.transform.position);
    }

    //Every frame
    private void Update()
    {
        //If in range, press f near gate, and not already travelling
        if (playerInRange && col.enabled && Input.GetKeyDown(KeyCode.F) && (!isInverse || rel.isBijective()))
        {
            //If inventory is empty
            if(playerBags.GetInventory() == null || playerBags.GetInventory().GetCardinality() == 0)
            {
                //Allow passage
                col.enabled = false;

                //Disable picking up
                playerInteract.enabled = false;

                //Start vault
                StartCoroutine(playerVault());
            }
            else
            {
                HUD.instance.inventory.Pulse(1.6f, 0.4f);
                AudioManager.instance.Play("Fail");
            }
           
        }

        //Only run following block in the main function gate, not also for the inverse
        if (!isInverse)
        {
            const float FADE_SPEED = 3f;
            //Display is on if mouse is hovering or player in range
            bool displayOn = mouseHover || playerInRange || inverse.playerInRange;

            //Gradually change alpha
            if (displayOn)
            {
                alpha = Mathf.Min(1, alpha + Time.deltaTime * FADE_SPEED);
            }
            else
            {
                alpha = Mathf.Max(0, alpha - Time.deltaTime);
            }
            display.SetAlpha(alpha);
        

            //Change line alpha
            Color endCol = line.endColor;
            endCol.a = alpha;
            line.endColor = endCol;
        }
        
    }

    //Animation for transforming items
    private IEnumerator transformItem(ItemPickup item)
    {
        //Get type of input item
        Item.Type type = item.itemType;

        AudioManager.instance.Play("FunctionCharge");

        //Absorb the item into the gate
        float t = 0;
        float absorbDuration = 0.3f;
        Vector2 originalPos = item.transform.position;

        //Move item towards centre of gate
        while(t < absorbDuration)
        {
            item.transform.position = Vector2.Lerp(originalPos, transform.position, t / absorbDuration);
            t += Time.deltaTime;
            yield return null;
        }

        //Destroy the item
        Destroy(item.gameObject);

        yield return new WaitForSeconds(0.2f);

        AudioManager.instance.Play("FunctionTransform");

        //Create new item
        Item.Type newType = isInverse ? rel.ApplyInverse(type) : rel.Apply(type); //Find output item

        ItemPickup newItem = Instantiate(Items.items[(int)newType]);
        newItem.transform.position = transform.position;
        newItem.launchedBy = gameObject;
        newItem.launchAngle = rel.transform.localRotation.eulerAngles.z * Mathf.Deg2Rad + (isInverse ? Mathf.PI : 0);

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
            vol.weight = Mathf.Min(1 -  t/vaultDuration, t/vaultDuration); //Fade post-processing in then out

            t += Time.deltaTime;
            yield return null;
        }

        vol.weight = 0;

        //Re-enable collider and picking up
        col.enabled = true;
        playerInteract.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseHover = false;
    }
}
