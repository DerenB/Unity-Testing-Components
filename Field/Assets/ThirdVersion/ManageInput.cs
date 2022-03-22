using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageInput : MonoBehaviour
{
    ControllingPlayer playerControls;
    Locomotion playerLocomotion;
    ManageAnimation animatorManager;
    

    // Movement
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    // Sprint Function
    public bool b_input;

    public float cameraInputX;
    public float cameraInputY;

    private void Awake()
    {
        playerLocomotion = GetComponent<Locomotion>();
        animatorManager = GetComponent<ManageAnimation>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new ControllingPlayer();
            
            // Movement
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            // Sprinting
            playerControls.PlayerAction.B.performed += i => b_input = true;
            playerControls.PlayerAction.B.canceled += i => b_input = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintInput();

        //Handle Jump Input

        // Handle Action Input
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintInput()
    {
        if(b_input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }
}
