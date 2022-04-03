using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorDomainSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public FunctionEditor editor;
    public EditorCodSlot codSlot;

    [SerializeField]
    private LineRenderer line;

    private bool held;
    public bool locked;

    public void OnPointerDown(PointerEventData eventData)
    {
        held = true;
        editor.currentSelectedDomainSlot = this;

        line.enabled = true;
        codSlot = null;
        editor.ResetLink(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        held = false;
        if (editor.currentSelectedDomainSlot == this) editor.currentSelectedDomainSlot = null;


        if(codSlot == null)
        {
            line.enabled = false;
            locked = false;
        }
        else
        {
            editor.CreateNewLink(this);
            locked = true;
        }
    }

    private Camera uiCamera;
    private void Start()
    {
        uiCamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (held || locked)
        {
            line.SetPosition(0, transform.position);
            if (codSlot != null)
            {
                line.SetPosition(1, codSlot.transform.position);
            }
            else
            {
                line.SetPosition(1, uiCamera.ScreenToWorldPoint(Input.mousePosition));
            }
            
            
        }
        

    }

}
