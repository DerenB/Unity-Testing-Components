using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    // Needs to be the same value as the max speed in ThirdPersonController
    private float maxSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Divides by maxSpeed to work with the animator
        animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
    }
}
