using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Input Action object
    PlayerControls playerControls;

    // Animation Script
    AnimatorManager animatorManager;

    [Header("Player Movement")]
    public float verticalMovementInput;
    public float horizontalMovementInput;
    private Vector2 movementInput;

    

    private void Awake()
    {
        // Calls the animation script
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        // Calls the Movement Input
        HandleMovementInput();

        // Handle Sprinting Input
    }

    private void HandleMovementInput()
    {
        // Gets the movement from input
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        
        // Applies the movement to the animation manager
        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput);
    }
}
