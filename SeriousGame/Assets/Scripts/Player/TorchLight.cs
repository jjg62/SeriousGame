using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

//Small light around player
public class TorchLight : MonoBehaviour
{
    private Light2D torchLight;
    private Light2D globalLight; //Reference to global light which illuminates whole level
    private CameraFollow mainCamera; //Reference to camera

    //Radius of torch light without/with holding space resp.
    private float defaultRadius = 4f;
    private float bigRadius = 7f;

    //Brightness of global light without/with holding space resp.
    private float defaultGlobalLight = 0.325f;
    private float bigGlobalLight = 0.65f;

    //Before first frame
    private void Start()
    {
        //Get references
        torchLight = GetComponent<Light2D>();
        globalLight = GameObject.FindWithTag("Global Light").GetComponent<Light2D>();
        mainCamera = Camera.main.GetComponent<CameraFollow>();
    }

    //Every frame
    float t = 0;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            t = Mathf.Min(1, t + Time.deltaTime * 4);
        }
        else
        {
            t = Mathf.Max(0, t - Time.deltaTime * 4);
        }

        //Linearly interpolate between current light settings and desired (from whether or not space is held)
        torchLight.pointLightOuterRadius = Mathf.Lerp(defaultRadius, bigRadius, t);
        globalLight.intensity = Mathf.Lerp(defaultGlobalLight, bigGlobalLight, t);
        mainCamera.SetCameraZoom(t);
    }
}
