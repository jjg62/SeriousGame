using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionEditZone : MonoBehaviour
{
    private bool inRange;
    private bool editing;

    private GameObject player;

    private FunctionEditor editorUI;

    [SerializeField]
    private NewFunctionGate gate;

    private void Start()
    {
        editorUI = HUD.instance.functionEditor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            inRange = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inRange = false;
        }
    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.F))
        {
            editing = !editing;

            player.GetComponent<PlayerMovement>().enabled = !editing;
           

            if (editing)
            {
                editorUI.gameObject.SetActive(true);
                editorUI.SetFunction(gate.rel);
            }
            else
            {
                gate.rel.links = editorUI.GetCreatedLinks();
                gate.UpdateDisplay();
                editorUI.gameObject.SetActive(false);
            }

        }
    }
}
