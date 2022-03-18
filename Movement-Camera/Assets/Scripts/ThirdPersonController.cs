using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    // INPUT FIELDS
    private ThirdPersonActionAssets playerActionAsset;
    private InputAction move;

    // MOVEMENT FIELDS
    private Rigidbody rb;

    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    // CAMERA
    [SerializeField] private Camera playerCamera;

    // ANIMATION
    private Animator animator;

    private void Awake()
    {
        // INITIALIZATION
        rb = this.GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonActionAssets();
        animator = this.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerActionAsset.Player.Jump.started += DoJump;

        // Attacking
        playerActionAsset.Player.Attack.started += DoAttack;
        
        move = playerActionAsset.Player.Move;
        playerActionAsset.Player.Enable();

    }

    private void OnDisable()
    {
        playerActionAsset.Player.Jump.started -= DoJump;

        // Attacking
        playerActionAsset.Player.Attack.started += DoAttack;

        playerActionAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);

        // Stops the player accelerating when input stops
        forceDirection = Vector3.zero;

        // Adjusts jump "floaty-ness"
        if(rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        // Limits the player horizontal velocity
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        // Adds rotation
        LookAt();
    }

    // CONTROL PLAYER ROTATION / DIRECTION FACING
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        // Checks if the player is giving input
        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }



    //JUMP FUNCTION
    private void DoJump(InputAction.CallbackContext obj)
    {
        if(IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    // FUNCTIONS TO CHECK IF PLAYER IS ON THE GROUND
    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        } else
        {
            return false;
        }
    }

    // ATTACKING
    private void DoAttack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("attack");
    }
}
