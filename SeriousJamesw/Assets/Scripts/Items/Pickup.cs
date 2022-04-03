using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Superclass for objects that can be picked up by player
public abstract class Pickup: Interactable
{
    //Need to implement what happens when picked up in children
    //public abstract void OnInteract(BagManager bagManager);

    private SpriteRenderer spr;
    private Rigidbody2D rb; //Rigidbody, controls physics
   

    //When created, at which angle to launch (from Function Gates)
    [HideInInspector]
    public float launchAngle;

    //The function gate this was launched by (if at all)
    [HideInInspector]
    public GameObject launchedBy;

    //Before first frame
    protected new void Start()
    {
        base.Start();
        //Get references
        spr = GetComponent<SpriteRenderer>();
        if (spr == null) spr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (launchedBy == null)
        {
            //Pick random angle/speed to drop item at
            float angle = Random.Range(0, 2 * Mathf.PI);
            float spdRand = Random.Range(0.5f, 1f);
            StartCoroutine(AfterDrop(angle, spdRand));
        }
        else
        {
            //If this pickup came out of a function gate, need to launch
            StartCoroutine(AfterDrop(launchAngle + Random.Range(-0.3f, 0.3f), 15));
        }

    }
   

    //Speed the pickup gradually drifts when dropped - prevents pickups from stacking on top and obscuring each other
    [SerializeField]
    float dropMoveSpeed;

    IEnumerator AfterDrop(float angle, float speed)
    {
        //Reduce opacity
        Color col = spr.color;
        col.a = 0.0f;
        spr.color = col;

        //Turn angle to unit vector
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        //Fade in opacity
        while (col.a < 1)
        {
            col.a += Time.deltaTime * 2;
            spr.color = col;

            //Move gradually
            rb.MovePosition(rb.position + dir * (1 - col.a) * dropMoveSpeed * speed * Time.fixedDeltaTime);

            yield return null;
        }
        col.a = 1f;
        spr.color = col;

        //Can now be picked up
        col.a = 1f;
        spr.color = col;
    }

}
