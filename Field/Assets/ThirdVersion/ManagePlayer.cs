using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePlayer : MonoBehaviour
{
    ManageInput inputManager;
    CameraManager cameraManager;
    Locomotion playerLocomotion;

    private void Awake()
    {
        inputManager = GetComponent<ManageInput>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<Locomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
    }
}
