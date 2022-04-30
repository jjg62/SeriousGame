using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Used when object needs a display for a condition
//In particular, GreenZone and RedZone are subclasses of this
public class HasDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IInventoryCheck
{
    //Prefab for the display gameobject
    [SerializeField]
    private DisplayCondition displayPref;

    //Where it should be in relation to this object
    [SerializeField]
    private Vector2 displayOffset;

    //References to line object, the display and the condition
    private LineRenderer line;
    [HideInInspector]
    public DisplayCondition display;
    [HideInInspector]
    public Condition con;

    //Distance the player needs to be to see the display
    [SerializeField]
    private float displayDistance = 3;

    private bool displayOn; //Is the display currently available
    private float alpha; //Alpha value of display
    private bool mouseHover; //Is mouse hovering over this object
    private Transform player; //Reference to player

    //When object is created, before Start
    protected void Awake()
    {
        con = GetComponent<Condition>();
        display = Instantiate(displayPref); //Instantiate the prefab and store it in display
    }

    //Before first frame
    protected void Start()
    {
        line = GetComponentInChildren<LineRenderer>();   
        if(con != null) display.SetCondition(con.GetCondString()); //Set the displayed condition
        display.transform.position = transform.position + (Vector3)displayOffset; //Set starting position
        player = GameObject.FindWithTag("Player").transform; //Get reference to player

        //Line should start at this object
        line.SetPosition(0, transform.position);
    }

    //Every frame
    protected void Update()
    {
        const float FADE_SPEED = 3f;

        //Display should be visible if mouse is hovering over or if player is within some distance
        displayOn = mouseHover || Vector2.Distance(transform.position, player.position) < displayDistance;

        //Gradually increase/decrease alpha
        if (displayOn)
        {
            alpha = Mathf.Min(1, alpha + Time.deltaTime * FADE_SPEED);
        }
        else
        {
            alpha = Mathf.Max(0, alpha - Time.deltaTime);
        }
        display.SetAlpha(alpha);

        //Line should end at display's condition
        line.SetPosition(1, display.transform.position);

        //Change line alpha
        Color endCol = line.endColor;
        endCol.a = alpha;
        line.endColor = endCol;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseHover = false;
    }

    public void InventoryCheck(Inventory inv)
    {
        //Update condition when ivnentory changes (in particular, the colour of player's bag)
        display.SetCondition(con.GetCondString());
    }
}
