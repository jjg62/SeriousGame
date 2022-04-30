using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionEditor : MonoBehaviour
{

    [SerializeField]
    private RawImage back;

    [SerializeField]
    private EditorDomainSlot domainSlotPref;

    [SerializeField]
    private EditorCodSlot codSlotPref;

    
    public EditorDomainSlot currentSelectedDomainSlot;

    private List<EditorDomainSlot> domainSlots;
    private List<EditorCodSlot> codSlots;
    private List<Relation.Link> links;
    private Relation relation;


    public List<Relation.Link> GetCreatedLinks() {
        return new List<Relation.Link>(links);
    }

    public void SetFunction(Relation f)
    {
        //Destroy all slots first
        if(domainSlots != null)
        {
            foreach (EditorDomainSlot s in domainSlots) Destroy(s.gameObject);
            foreach (EditorCodSlot s in codSlots) Destroy(s.gameObject);
        }


        relation = f;

        domainSlots = new List<EditorDomainSlot>();
        codSlots = new List<EditorCodSlot>();
        links = new List<Relation.Link>(f.links);

        float slotHeight = domainSlotPref.transform.localScale.y;
        float domainHeight = f.domain.Count * slotHeight;
        float yPos = transform.position.y - domainHeight / 2.0f;


        foreach (Item.Type i in f.domain)
        {
            yPos += slotHeight / 2.0f;

            EditorDomainSlot newSlot = Instantiate(domainSlotPref);
            newSlot.transform.SetParent(transform, false);
            newSlot.editor = this;
            newSlot.transform.position = new Vector3(transform.position.x - 1.0f, yPos, 0);
            newSlot.GetComponentInChildren<SpriteRenderer>().sprite = Icons.icons[(char)((int)i + 48)];
            domainSlots.Add(newSlot);

            yPos += slotHeight / 2.0f;
        }

        float codomainHeight = f.codomain.Count * slotHeight;
        yPos = transform.position.y - codomainHeight / 2.0f;

        foreach (Item.Type i in f.codomain)
        {
            yPos += slotHeight / 2.0f;

            EditorCodSlot newSlot = Instantiate(codSlotPref, transform);
            newSlot.editor = this;
            newSlot.transform.position = new Vector2(transform.position.x + 1.0f, yPos);
            newSlot.GetComponentInChildren<SpriteRenderer>().sprite = Icons.icons[(char)((int)i + 48)];
            codSlots.Add(newSlot);

            yPos += slotHeight / 2.0f;
        }

        back.transform.localScale = new Vector2(back.transform.localScale.x, Mathf.Max(codomainHeight, domainHeight));
        //border.transform.localScale = back.transform.localScale + new Vector3(0.05f, 0.05f, 0);

        foreach(Relation.Link l in links)
        {
            domainSlots[l.input].codSlot = codSlots[l.output];
            domainSlots[l.input].locked = true;
        }
    }

    public void ResetLink(EditorDomainSlot slot)
    {

        int domainIndex = domainSlots.IndexOf(slot);
        //Remove link with domainIndex as in
        foreach(Relation.Link l in links)
        {
            if (l.input == domainIndex)
            {
                links.Remove(l);
                return;
            }
        }
    }
    
    //This isn't close to working yet but is an outline
    public void CreateNewLink(EditorDomainSlot slot)
    {
        int domainIndex = domainSlots.IndexOf(slot);
        int codIndex = codSlots.IndexOf(slot.codSlot);
        Relation.Link newLink = new Relation.Link();
        newLink.input = domainIndex;
        newLink.output = codIndex;
        links.Add(newLink);
    }
}
