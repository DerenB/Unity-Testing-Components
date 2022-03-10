using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    // Character controller, motor that drives the player
    public CharacterController controller;

    // Camera reference
    public Transform cam;

    // Speed
    public float speed = 6f;

    // Smooth the turn time
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Stores Direction
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude > 0.1f)
        {
            // Points character in the direction of the camera
            // Atan2 is a math function that returns the angle between the x axis
            // and the vector starting at 0 and terminating at x,y
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // Smoothes out the rotation of the character
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}


/*
 * SOURCE VIDEO
 * https://youtu.be/4HpC--2iowE
 */
