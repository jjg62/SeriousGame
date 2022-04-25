using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//The main purpose of this script is to take a string and convert it to icons that are displayed
public class DisplayCondition : MonoBehaviour
{
    //Sprites of the display
    [SerializeField]
    private SpriteRenderer back;
    [SerializeField]
    private SpriteRenderer border;

    //Prefab for icons
    [SerializeField]
    private GameObject charSlot;

    private List<GameObject> charSlots;

    //Lists of characters that should take half or double the space
    private List<char> halfWidthChars;
    private List<char> doubleWidthChars;

    private float startY;

    //Before Start, when object is created
    private void Awake()
    {
        //Initialise the arrays
        charSlots = new List<GameObject>();
        halfWidthChars = new List<char> { '{', '}', '|' };
        doubleWidthChars = new List<char> { '~' };
    }

    //Before first frame
    private void Start()
    {
        //Store the original y position of the display
        startY = transform.position.y;
    }
    
    private float t;
    //Every frame
    private void Update()
    {
        //Display should slightly oscilate up and down
        const float amplitude = 0.06f;

        t += 2 * Time.deltaTime;
        if (t > 2 * Mathf.PI) t -= 2 * Mathf.PI;

        transform.position = new Vector2(transform.position.x, startY + amplitude * Mathf.Sin(t));
    }

    //Get the width the display should be from the condition string
    private float GetStringWidth(string condition)
    {
        float total = 0;
        bool textMode = false;
        bool colourMode = false;

        foreach(char c in condition)
        {
            //A slash in the condition string toggles text mode, isn't actually an icon itself
            if(c == '/')
            {
                textMode = !textMode;
            }

            //An @ in the string toggles colour mode
            if (c == '@') {
                colourMode = !colourMode;
            }
            //Otherwise, add to total the width of this character's icon
            else if (!textMode && !colourMode)
            {
                if (halfWidthChars.Contains(c))
                {
                    total += charSlot.transform.localScale.x / 2.0f;
                }
                else if (doubleWidthChars.Contains(c))
                {
                    total += charSlot.transform.localScale.x * 2.0f;
                }
                else
                {
                    total += charSlot.transform.localScale.x;
                }
            }
        }
        return total;
    }

    //Given a string, set up the display with the right icons
    public void SetCondition(string condition)
    {
        //Get current alpha
        float a = back.color.a;


        //Destroy currently displayed icons
        foreach(GameObject s in charSlots)
        {
            Destroy(s);
        }
        charSlots.Clear();

        //Get width of the icon prefab
        float charWidth = charSlot.transform.localScale.x;

        //Set width of back sprite depending on width of string
        back.transform.localScale = new Vector2(GetStringWidth(condition), transform.localScale.y);
        border.transform.localScale = back.transform.localScale + new Vector3(0.05f, 0.05f, 0);

        float xPos = transform.position.x - GetStringWidth(condition)/2.0f;

        //While text mode is on, characters are written as text to the display instead of converted to icons
        //Toggled by typing / in the condition string
        bool textMode = false;
        TextMeshPro currentTextSlot = null;

        //While colour mode is on, the string is describing the RGB code for the desired colour of previous icon
        //Toggled by typing @ in the condition string
        bool colourMode = false;
        string colour = "";

        GameObject newChar = null;
        foreach (char c in condition)
        {
            if (colourMode)
            {
                if (c == '@')
                {
                    //If already in colour mode and this character is @, turn it off
                    colourMode = false;

                    //Convert RGB string to a colour
                    Color parsedCol;
                    if (ColorUtility.TryParseHtmlString(colour, out parsedCol)){
                        newChar.GetComponentInChildren<SpriteRenderer>().color = parsedCol; //Set colour of sprite to the parsed colour
                    }
                    else
                    {
                        Debug.LogWarning("WARNING: Colour parse failed in display!");
                    }
                }
                //While still in colour mode, add characters to the colour string
                else colour += c;
            }
            else if (textMode)
            {
                //In text mode: disable if another / is read, otherwise keep track of characters in a string
                if (c == '/') textMode = false;
                else currentTextSlot.text += c;
            }
            else
            {             
                //Not currently in text mdoe or colour mode
                if(c == '@')
                {
                    //Start colour mode
                    colourMode = true;
                    colour = "#"; //Initialise colour string
                    continue; //Next character
                }

                //Get width of this icon
                float w = halfWidthChars.Contains(c) ? charWidth / 2.0f : (doubleWidthChars.Contains(c) ? charWidth * 2.0f : charWidth);

                xPos += w / 2.0f;
                //Create the new icon at xPos
                newChar = Instantiate(charSlot, transform);
                newChar.transform.position = new Vector2(xPos, transform.position.y);
                charSlots.Add(newChar);
                xPos += w / 2.0f;

                if (c == '/')
                {
                    //Start text mode
                    textMode = true;
                    currentTextSlot = newChar.GetComponentInChildren<TextMeshPro>();
                    currentTextSlot.text = "";
                }
                else
                {
                    //Use dictionary in Icons to find the icon for this character
                    newChar.GetComponentInChildren<SpriteRenderer>().sprite = Icons.icons[c];
                }
                  
            }
            
        }
        SetAlpha(a);
    }

    //Stop any flashing effects and start a new one
    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCondition());
    }

    //In inpsector, define usual colour, and colour that is flashed
    [SerializeField]
    private Color flashColour;
    [SerializeField]
    private Color normalColour;

    //Flash effect
    IEnumerator FlashCondition()
    {
        back.color = normalColour;

        float t = 0;
        //Durations of colour changes
        const float warmUp = 0.1f;
        const float coolDown = 0.6f;

        //Linearly interpolate colour to flashColour
        while(t < warmUp)
        {
            t += Time.deltaTime;
            back.color = Color.Lerp(normalColour, flashColour, t / warmUp);
            yield return null;
        }
        back.color = flashColour;

        //Linearly interpolate back to normalColour
        t = 0;
        while(t < coolDown)
        {
            t += Time.deltaTime;
            back.color = Color.Lerp(flashColour, normalColour, t / coolDown);
            yield return null;
        }
    }

    //Set alpha of all sprites on the display
    public void SetAlpha(float a)
    {
        //Back sprites
        SetAlpha(back, a);
        SetAlpha(border, a);

        //Icons / Text on display
        foreach (GameObject slot in charSlots)
        {
            SetAlpha(slot.GetComponentInChildren<SpriteRenderer>(), a);
            slot.GetComponentInChildren<TextMeshPro>().alpha = a;
        }
    }

    //Set alpha of a sprite
    private void SetAlpha(SpriteRenderer sprite, float a)
    {
        Color c = sprite.color;
        c.a = a;
        sprite.color = c;
    }
}
