using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//One slot that can display an item on the HUD
public class InventorySlot : MonoBehaviour
{
    //Image that can display item type
    [SerializeField]
    private RawImage icon;

    //Text that can display item quantity
    [SerializeField]
    private TextMeshProUGUI count;

    //When created, store default size of icon
    private Vector2 defaultSize;
    private void Start()
    {
        defaultSize = icon.transform.localScale;
    }

    //Update image and quantity
    public void Set(Sprite icon, int count)
    {
        //Set image
        this.icon.enabled = true;
        this.icon.texture = icon.texture;
        
        //Only display quantity if is it more than 1
        if(count > 1)
        {
            this.count.text = count.ToString();
        }
        else
        {
            this.count.text = "";
        }

    }

    //When slot should no longer show an item
    public void Clear()
    {
        //Disable image and quantity
        icon.enabled = false;
        count.text = "";
    }

    //Change size of image for this slot
    IEnumerator PulseAnimation(float size, float duration)
    {
        Vector2 newScale = new Vector2(size, size);

        //Instantly set scale to target
        icon.transform.localScale = newScale;

        //Then gradually decrease back to normal scale
        float t = 0;
        while(t < duration)
        {
            icon.transform.localScale = Vector2.Lerp(newScale, defaultSize, t/duration);
            t += Time.deltaTime;
            yield return null;
        }

        icon.transform.localScale = defaultSize;
    }

    //Stop any animations and start a new pulse animation
    public void Pulse(float size, float duration)
    {
        StopAllCoroutines();

        StartCoroutine(PulseAnimation(size, duration));
    }
}
