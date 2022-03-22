using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerCamera playerCamera;

    private void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    private void LateUpdate()
    {
        playerCamera.HandleAllCameraMovement();
    }
}
