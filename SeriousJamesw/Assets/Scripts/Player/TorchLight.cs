using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

//Small light around player
public class TorchLight : MonoBehaviour
{
    private Light2D torchLight;
    private Light2D globalLight; //Reference to global light which illuminates whole level

    //Radius of torch light without/with holding space resp.
    private float defaultRadius = 4f;
    private float bigRadius = 7f;

    //Brightness of global light without/with holding space resp.
    private float defaultGlobalLight = 0.35f;
    private float bigGlobalLight = 0.65f;

    //Before first frame
    private void Start()
    {
        //Get references
        torchLight = GetComponent<Light2D>();
        globalLight = GameObject.FindWithTag("Global Light").GetComponent<Light2D>();
    }

    //Every frame
    private void Update()
    {
        //Linearly interpolate between current light settings and desired (from whether or not space is held)
        torchLight.pointLightOuterRadius = Mathf.Lerp(torchLight.pointLightOuterRadius, Input.GetKey(KeyCode.Space) ? bigRadius : defaultRadius, Time.deltaTime * 4);
        globalLight.intensity = Mathf.Lerp(globalLight.intensity, Input.GetKey(KeyCode.Space) ? bigGlobalLight : defaultGlobalLight, Time.deltaTime * 4);
    }
}
