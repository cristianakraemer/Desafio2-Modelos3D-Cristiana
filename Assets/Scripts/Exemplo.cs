using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exemplo : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private bool isDancing = false;
    private Quaternion targetRotation;

    public float rotationSpeed = 6f;
    public float walkSpeed = 1.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        animator.SetBool("Walk", movement != Vector3.zero != isDancing);
        animator.SetBool("Dance", isDancing);

        if (Input.GetKeyDown(KeyCode.R))
        {
            isDancing = !isDancing;
            animator.SetBool("Walk", false);
            animator.SetBool("Dance", isDancing);
        }

        if (movement != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (Input.GetKeyUp(KeyCode.R))
        {
            isDancing = false;
            animator.SetBool("Dance", false);
            animator.SetBool("Walk", true);
        }
    }
}
