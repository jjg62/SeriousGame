using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Get controls from keyboard and move player
public class PlayerMovement : MonoBehaviour
{
    //Rigidbody - handles physics
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed; //Current move speed
    private Vector2 movement; //Vector for currently moving direction

    private float defMoveSpeed; //Default move speed

    private Animator anim; //Reference to animator


    private void Start()
    {
        //Get references
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //Set default move speed to the initial value of move speed as set in inspector
        defMoveSpeed = moveSpeed;  
    }

    private bool moving;
    private bool torchSoundPlayed;
    private void Update()
    {
        //Get input from keyboard/controller
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Update animator booleans depending on last horizontal direction moved
        if(movement.x > 0) anim.SetBool("FaceRight", true);
        else if(movement.x < 0) anim.SetBool("FaceRight", false);


        if (Input.GetKey(KeyCode.Space))
        {
            //Stop player from moving while holding space
            moveSpeed = 0;
            moving = false;


            if (!torchSoundPlayed)
            {
                //Play the torch sound effect once and play torch animation
                anim.SetBool("Torch", true);
                AudioManager.instance.Stop("Footstep");
                AudioManager.instance.Stop("Torch");
                AudioManager.instance.Play("Torch");
                torchSoundPlayed = true;
            }
            
        }
        else
        {
            torchSoundPlayed = false;

            //Move speed returns to normal
            moveSpeed = defMoveSpeed;

            //Finish torch animation
            anim.SetBool("Torch", false);

            //Tell animator how fast player is moving
            anim.SetFloat("Speed", movement.magnitude);

            //Footstep sound effect control
            //It loops, so only need to start/stop it once, hence the moving variable
            if (!moving && movement.magnitude > 0)
            {
                moving = true;
                AudioManager.instance.Play("Footstep");
            }
            else if (moving && movement.magnitude < 0.1f)
            {
                moving = false;
                AudioManager.instance.Stop("Footstep");

            }
        }
    }

    private void FixedUpdate()
    {
        //Apply inputs to rigidbody, moving the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
