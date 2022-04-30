using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Make the new drop bag button flash until the player tries it
public class DropBagTutorial : MonoBehaviour
{
    //The red flash
    private RawImage img;

    private float t = 0;

    private void Start()
    {
        img = GetComponent<RawImage>();
    }

    private void Update()
    {
        //Make the sprite flash on a 1 second period
        t = (t + Time.deltaTime) % 1;
        Color c = img.color;
        c.a = t;
        img.color = c;

        //Destroy when player presses the drop button
        if (Input.GetKeyDown(KeyCode.Q)) Destroy(gameObject);
    }
}
