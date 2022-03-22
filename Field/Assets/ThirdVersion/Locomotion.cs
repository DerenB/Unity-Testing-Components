using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    ManagePlayer playerManager;
    ManageAnimation animationManager;
    ManageInput inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidBody;

    [Header("Falling")]
    public float inAirTime;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 9;
    public float rotationSpeed = 15;

    private void Awake()
    {
        playerManager = GetComponent<ManagePlayer>();
        animationManager = GetComponent<ManageAnimation>();
        inputManager = GetComponent<ManageInput>();
        playerRigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (playerManager.isInteracting)
        {
            return;
        }

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Movement Input
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if(isSprinting)
        {
            moveDirection *= sprintingSpeed;
        }
        else
        {
            // Various Moving speeds
            if (inputManager.moveAmount > 0.5f)
            {
                moveDirection *= runningSpeed;
            }
            else
            {
                moveDirection *= walkingSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidBody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        if(!isGrounded)
        {
            //Debug.Log("YOU FELL");
            if (!playerManager.isInteracting)
            {
                //Debug.Log("animate - - - fall");
                animationManager.PlayTargetAnimation("Falling", true);
            }

            inAirTime = inAirTime + Time.deltaTime;
            playerRigidBody.AddForce(transform.forward * leapingVelocity);
            playerRigidBody.AddForce(-Vector3.up * fallingVelocity * inAirTime);
        }

        if(Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            //Debug.Log("YOU HIT THE GROUND");
            if(!isGrounded && !playerManager.isInteracting)
            {
                //Debug.Log("animate - - - land");
                animationManager.PlayTargetAnimation("Land", true);
            }

            inAirTime = 0;
            isGrounded = true;
        }
        else
        {
            //Debug.Log("STILL FALLING ...... ");
            isGrounded = false;
        }
    }
}
