﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make camera follow an object (the player)
public class CameraFollow : MonoBehaviour
{
    //The object to follow
    [SerializeField]
    private Transform target;

    //Position of center of level
    [SerializeField]
    private Vector2 levelCenter;

    //How quickly the camera moves to match target
    [SerializeField]
    private float followSpeed;

    //Store z the camera starts at, for movement later
    private float startingZ;

    //Reference to Camera component
    private Camera cam;

    //Zoom level usually
    private float defaultZoom = 5f;

    //Zoom level when Space held
    [SerializeField]
    private float spaceZoom = 7f;

    //Before first frame
    private void Start()
    {
        startingZ = transform.position.z; //Get value for startingZ
        cam = GetComponent<Camera>();

        //Move camera to position of target
        Vector3 pos = target.position;
        pos.z = startingZ; //But stay at startingZ
        transform.position = pos;
    }

    //Every frame, aligned with physics engine for smoother movement
    private void FixedUpdate()
    {
        Vector3 cameraPos;

        //Get target position - on player usually, halfway between player and level center when space held
        Vector2 t = Input.GetKey(KeyCode.Space) ? ((Vector2)target.position + levelCenter)/2.0f : (Vector2)target.position;

        //Linearly interpolate between camera pos and player pos
        cameraPos = Vector2.Lerp(transform.position, t, Time.fixedDeltaTime * followSpeed);
        cameraPos.z = startingZ; //But stay at startingZ
        transform.position = cameraPos;

        //Change zoom of camera depending on if space held
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Input.GetKey(KeyCode.Space) ? spaceZoom : defaultZoom, Time.deltaTime * 4);
    }
}
