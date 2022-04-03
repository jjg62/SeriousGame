using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make sprite's transparency increase and decrease over time
public class PulsateSprite : MonoBehaviour
{
    //Get reference to the sprite
    private SpriteRenderer spr;
    //Before first frame
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    //Period of pulse
    [SerializeField]
    private float period = Mathf.PI;

    //Timer variable (also start time)
    [SerializeField]
    private float t;

    //Every frame
    private void Update()
    {
        //Increase timer, modulo period
        t = (t + Time.deltaTime) % period;

        //Change alpha according to sin function
        Color c = spr.color;
        c.a = Mathf.Sin(t * Mathf.PI/period);
        spr.color = c;

    }
}
