using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    InputManager inputManager;

    [Header("Camera Transform")]
    public Transform playerCamera;

    // How fast the character will rotate
    [Header("Movement Speed")]
    public float rotationSpeed = 1.5f;

    [Header("Rotation Variables")]
    Quaternion targetRotation; // The place we want to rotate
    Quaternion playerRotation; // The current rotation

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }

    public void HandleAllLocomotion()
    {
        // Handle Rotation
        HandleRotation();

        // Handle Falling
    }

    private void HandleRotation()
    {
        targetRotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
        playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if(inputManager.verticalMovementInput != 0 || inputManager.horizontalMovementInput != 0)
        {
            transform.rotation = playerRotation;
        }
    }
}
