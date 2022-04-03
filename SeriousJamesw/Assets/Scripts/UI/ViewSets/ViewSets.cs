using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HUD element for showing all bags and their contents
public class ViewSets : MonoBehaviour
{
    //Prefab for a row in this menu
    [SerializeField]
    private ViewSetsRow rowPref;

    //List of current rows
    private List<ViewSetsRow> rows;

    //Update contents of all sets
    public void Refresh()
    {
        //Find player
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        //This function is called by BagPickup.OnDestroy(), so need to be careful that all other objects haven't been destroyed too 
        //i.e. at the end of a level


        //Delete old rows
        if (rows != null)
        {
            foreach (ViewSetsRow row in rows)
            {
                if(row.gameObject != null) Destroy(row.gameObject);
            }
        }
        rows = new List<ViewSetsRow>();

       
        //Get IDs of all bags the player has picked up
        List<int> keys = player.GetComponent<BagManager>().GetKeys();

        float h = rowPref.GetComponent<RectTransform>().rect.height; //Height of a row
        float currentY = 0;

        //First row should always be universal set
        currentY -= h / 2.0f;
        ViewSetsRow universeRow = Instantiate(rowPref, transform);
        universeRow.invID = -1;
        universeRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, currentY);
        currentY -= h / 2.0f;

        //For each bag that had been picked up
        foreach (int key in keys)
        {
            currentY -= h / 2.0f;
            //Create new row
            ViewSetsRow newRow = Instantiate(rowPref, transform);
            newRow.invID = key; //For this bag
            newRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, currentY);

            rows.Add(newRow);
            currentY -= h / 2.0f;
        }

        //Change size of menu
        float totalHeight = (keys.Count+1) * h;
        RectTransform tr = GetComponent<RectTransform>();
        tr.sizeDelta = new Vector2(600, totalHeight);
        tr.anchoredPosition = new Vector2(-80, -24 - totalHeight/2);
    }
}
