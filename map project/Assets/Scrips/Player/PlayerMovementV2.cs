using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovementV2 : MonoBehaviour
{
    public Player player;
    public CharacterController controller;

    // base movement
    public Vector3 moveDir;
    private float speed;
    private float gravity;

    // ground check
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public List<LayerMask> groundMasks;

    bool isGrounded;

    Vector3 velocity;



    void Start()
    {
        speed = player.BaseSpeed.Value;
        gravity = -9.81f * 4;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCharacterMovement();
        HandleJump();
        //HandleHookStart();
    }

    private void HandleCharacterMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDir = transform.right * x + transform.forward * z;

        if (Input.GetKey("left shift"))
        {
            speed = player.SprintSpeed.Value;
        }
        else
        {
            speed = player.BaseSpeed.Value;
        }

        //Debug.Log($"Player speed is {speed}");

        controller.Move(moveDir * speed * Time.deltaTime);


    }

    private void HandleJump()
    {
        isGrounded = groundMasks.Any(groundMask => Physics.CheckSphere(groundCheck.position, groundDistance, groundMask));

        //Debug.Log($"isGrounded - {isGrounded}");

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(player.JumpHeight.Value * -2f * gravity);
        }


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleHookStart()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    //Physics.Raycast()
        //}
    }
}
