using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//Used in the main menu
//When this UI object is hovered over with the mouse, show a text object
public class ButtonHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Reference to the text object
    [SerializeField]
    private TextMeshProUGUI textObj;

    //Text to show
    [SerializeField]
    private string text;

    //Colour for text
    [SerializeField]
    private Color colour;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Set text, colour and make text visible
        textObj.text = text;
        textObj.color = colour;
        textObj.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Hide text
        textObj.gameObject.SetActive(false);
    }
}
