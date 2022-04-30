using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorCodSlot : MonoBehaviour
{
    public FunctionEditor editor;

    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(editor.currentSelectedDomainSlot != null)
        {
            editor.currentSelectedDomainSlot.codSlot = this;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (editor.currentSelectedDomainSlot != null && editor.currentSelectedDomainSlot.codSlot == this)
        {
            editor.currentSelectedDomainSlot.codSlot = null;
        }
    }
    ADD IN INTERFACES AGAIN
    */

    private Camera uiCamera;
    private void Start()
    {
        uiCamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
    }
   
    private void Update()
    {
        if(editor.currentSelectedDomainSlot != null)
        {
            if(Vector2.Distance(uiCamera.ScreenToWorldPoint(Input.mousePosition), transform.position) < 0.6f)
            {
                editor.currentSelectedDomainSlot.codSlot = this;
            }
            else if(editor.currentSelectedDomainSlot.codSlot == this)
            {
                editor.currentSelectedDomainSlot.codSlot = null;
            }
            
        }
    }
}
