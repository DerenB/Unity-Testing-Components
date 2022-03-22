using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    // Input Action object
    PlayerControls playerControls;
    private InputAction move;

    // Animation Script
    AnimatorManager animatorManager;

    // Movement Fields
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 2.0f;
    [SerializeField] private float maxSpeed = 1f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField] private Camera playerCamera;

    // Jump Variables
    public bool onGround;
    public Vector3 jump;
    public int jumpNum;

    private void Awake()
    {
        // Calls the animation script
        animatorManager = GetComponent<AnimatorManager>();

        rb = this.GetComponent<Rigidbody>(); // A
        playerControls = new PlayerControls(); // A
    }

    private void Start()
    {
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    private void OnEnable()
    {
        playerControls.PlayerMovement.Jump.started += DoJump;
        move = playerControls.PlayerMovement.Movement;
        playerControls.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        playerControls.PlayerMovement.Jump.started -= DoJump;
        playerControls.PlayerMovement.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;
        
        // Gradual falling speed
        if(rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.deltaTime;
        }

        // Limits the player's speed
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        // Checks for player input
        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            // Controls the rotation of the rigidbody
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        // Checks if the player is on the ground
        /*
        if(IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
        */

        if(onGround && jumpNum == 1)
        {
            Debug.Log("Jumped");
            jumpNum = 0;
            onGround = false;
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
        if(jumpNum == 0)
        {
            Debug.Log("No jumps avail");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        jumpNum = 1;
        onGround = true;
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
