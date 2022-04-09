using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroy this game object after a certain amount of time
public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float time = 1;

    private void Start()
    {
        //Call Destroy after some time
        Invoke("Destroy", time);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
