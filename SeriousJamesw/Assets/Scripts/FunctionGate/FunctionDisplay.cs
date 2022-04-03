using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Display domain, codomain and links of a function
public class FunctionDisplay : MonoBehaviour
{
    //Prefab for item icon in domain/codomain
    [SerializeField]
    private GameObject slotPrefab;

    //Prefab for line (link) object
    [SerializeField]
    private LineRenderer linePrefab;

    //Back sprites
    [SerializeField]
    private SpriteRenderer back;
    [SerializeField]
    private SpriteRenderer border;

    //Lists of domain icons / codomain / links
    private List<GameObject> domainSlots;
    private List<GameObject> codomainSlots;
    private List<LineRenderer> links;

    //Alpha of display
    private float alpha = 1f;

    //Colours for back and border, when function is one-way and bijective
    [SerializeField]
    private Color backColour1;
    [SerializeField]
    private Color backColourBij;
    [SerializeField]
    private Color frameColour1;
    [SerializeField]
    private Color frameColourBij;

    //Sprite for forward-facing arrow
    [SerializeField]
    private SpriteRenderer forwardArrow;

    //Sprite for inverse arrow
    [SerializeField]
    private SpriteRenderer backwardArrow;


    //Set up the display given a function
    public void SetFunction(Relation f)
    {
        //Destroy all previous prefabs first
        if (domainSlots != null)
        {
            foreach (GameObject s in domainSlots) Destroy(s);
            foreach (GameObject s in codomainSlots) Destroy(s);
            foreach (LineRenderer l in links) Destroy(l.gameObject);
        }

        //Initialise lists
        domainSlots = new List<GameObject>();
        codomainSlots = new List<GameObject>();
        links = new List<LineRenderer>();

        float slotHeight = slotPrefab.transform.localScale.y;

        //Calculate height needed to fit domain
        float domainHeight = f.domain.Count * slotHeight;
        float yPos = transform.position.y - domainHeight / 2.0f;

        //First, create icons for domain
        foreach(Item.Type i in f.domain)
        {
            yPos += slotHeight / 2.0f;

            //Instantiate icon at yPos
            GameObject newSlot = Instantiate(slotPrefab, transform);
            newSlot.transform.position = new Vector2(transform.position.x -0.6f, yPos);
            newSlot.GetComponentInChildren<SpriteRenderer>().sprite = Icons.icons[(char)((int)i+48)];
            domainSlots.Add(newSlot);

            yPos += slotHeight / 2.0f;
        }

        //Calculate height needed to fit codomain
        float codomainHeight = f.codomain.Count * slotHeight;
        yPos = transform.position.y - codomainHeight / 2.0f;

        //Create icons for codomain
        foreach(Item.Type i in f.codomain)
        {
            yPos += slotHeight / 2.0f;

            //Instantiate icon at yPos
            GameObject newSlot = Instantiate(slotPrefab, transform);
            newSlot.transform.position = new Vector2(transform.position.x + 0.6f, yPos);
            newSlot.GetComponentInChildren<SpriteRenderer>().sprite = Icons.icons[(char)((int)i + 48)];
            codomainSlots.Add(newSlot);

            yPos += slotHeight / 2.0f;
        }

        //Box needs to fit both domain and codomain
        float height = Mathf.Max(codomainHeight, domainHeight);

        back.transform.localScale = new Vector2(back.transform.localScale.x, height + 1);
        border.transform.localScale = back.transform.localScale + new Vector3(0.05f, 0.05f, 0);

        //Change colour depending on whether f is bijective
        bool bijective = f.isBijective();
        back.color = bijective ? backColourBij : (f.isFunction() ? backColour1 : Color.gray);
        border.color = bijective ? frameColourBij : (f.isFunction() ? frameColour1 : Color.gray);

        //Arrows on display
        forwardArrow.transform.localPosition = new Vector2(0, (height+0.5f) / 2.0f);
        backwardArrow.transform.localPosition = new Vector2(0, -(height+0.5f) / 2.0f);

        //Only display backwardArrow if function is bijective
        backwardArrow.gameObject.SetActive(bijective);

        //Create a line for each link
        foreach (Relation.Link l in f.links)
        {
            LineRenderer newLine = Instantiate(linePrefab, transform);
            links.Add(newLine);
            newLine.SetPosition(0, domainSlots[l.input].transform.position);
            newLine.SetPosition(1, codomainSlots[l.output].transform.position);
        }

        SetAlpha(alpha);
    }

    //Set alpha of all sprites on the display
    public void SetAlpha(float a)
    {
        alpha = a;

        //Back sprites and arrows
        SetAlpha(back, a);
        SetAlpha(border, a);
        SetAlpha(forwardArrow, a);
        SetAlpha(backwardArrow, a);

        //Icons in domain
        foreach (GameObject slot in domainSlots)
        {
            SetAlpha(slot.GetComponentInChildren<SpriteRenderer>(), a);
        }

        //Icons in codomain
        foreach (GameObject slot in codomainSlots)
        {
            SetAlpha(slot.GetComponentInChildren<SpriteRenderer>(), a);
        }

        //Lines
        foreach(LineRenderer l in links)
        {
            Color startCol = l.startColor;
            Color endCol = l.endColor;
            startCol.a = a;
            endCol.a = a;
            l.startColor = startCol;
            l.endColor = endCol;
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
