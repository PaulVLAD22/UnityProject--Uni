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

    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;

    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;

    private float cameraVerticalAngle;
    private float characterVelocityY;
    private Vector3 characterVelocityMomentum;
    private Camera playerCamera;
    private CameraFov cameraFov;
    private ParticleSystem speedLinesParticleSystem;
    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;


    void Start()
    {
        speed = player.BaseSpeed.Value;
        gravity = -9.81f * 4;
    }

    private void Awake()
    {
        playerCamera = transform.Find("Camera rotation").Find("CameraRecoil").Find("Main Camera").GetComponent<Camera>();
        cameraFov = playerCamera.GetComponent<CameraFov>();
        speedLinesParticleSystem = transform.Find("Camera rotation").Find("CameraRecoil").Find("Main Camera").Find("Particle System").GetComponent<ParticleSystem>();
        Cursor.lockState = CursorLockMode.Locked;
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
        speedLinesParticleSystem.Stop();
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                HandleJump();
                HandleCharacterMovement();
                HandleHookshotStart();
                break;
            case State.HookshotThrown:
                HandleHookshotThrow();
                HandleJump();
                HandleCharacterMovement();
                break;
            case State.HookshotFlyingPlayer:
                HandleJump();
                HandleHookshotMovement();
                break;
        }
    }

    private void HandleCharacterMovement()
    {
        //Debug.Log("Moving...");
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

    // HOOKSHOT STUFF


    private enum State
    {
        Normal,
        HookshotThrown,
        HookshotFlyingPlayer,
    }


    private void ResetGravityEffect()
    {
        characterVelocityY = 0f;
    }

    private void HandleHookshotStart()
    {
        if (TestInputDownHookshot())
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit))
            {
                // Hit something
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                state = State.HookshotThrown;
            }
        }
    }

    private void HandleHookshotThrow()
    {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 500f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFlyingPlayer;
            cameraFov.SetCameraFov(HOOKSHOT_FOV);
            speedLinesParticleSystem.Play();
        }
    }

    private void HandleHookshotMovement()
    {
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 5f;

        // Move Character Controller
        controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        float reachedHookshotPositionDistance = 1f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            // Reached Hookshot Position
            StopHookshot();
        }

        if (TestInputDownHookshot())
        {
            // Cancel Hookshot
            StopHookshot();
        }

        if (TestInputJump())
        {
            // Cancelled with Jump
            float momentumExtraSpeed = 7f;
            characterVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;
            float jumpSpeed = 40f;
            characterVelocityMomentum += Vector3.up * jumpSpeed;
            StopHookshot();
        }
    }

    private void StopHookshot()
    {
        Debug.Log("Hookshot stopped");
        state = State.Normal;
        ResetGravityEffect();
        hookshotTransform.gameObject.SetActive(false);
        cameraFov.SetCameraFov(NORMAL_FOV);
        speedLinesParticleSystem.Stop();
    }

    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private bool TestInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
