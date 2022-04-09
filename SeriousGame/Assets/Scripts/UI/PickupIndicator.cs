using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Indicator showing player they can interact with an item
public class PickupIndicator : MonoBehaviour
{
    //Whether the indicator should show
    private bool on;

    private float scale;

    //Toggle whether indicator is showing, resetting scale first if it should
    public void Activate(bool on)
    {
        this.on = on;

        if (on) scale = 0;
    }

    //Gradually change indicator size depending on whether it's activated
    private void Update()
    {
        const float GROW_SPEED = 5f;
        if (on)
        {
            scale = Mathf.Min(1, scale + Time.deltaTime * GROW_SPEED);
        }
        else
        {
            scale = Mathf.Max(0, scale - Time.deltaTime * GROW_SPEED);
        }

        transform.localScale = new Vector2(scale, scale);
    }


}
