using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private bool isPunching = false;
    private Quaternion targetRotation;

    public float rotationSpeed = 6f;
    public float walkSpeed = 1.9f;
    public float punchSpeed = 10f;

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

        float speed = isPunching ? punchSpeed : walkSpeed;
        bool isWalking = movement != Vector3.zero;

        animator.SetBool("MWalk", isWalking);
        animator.SetBool("MSlash", isPunching);

        // Verifica se a tecla "Q" foi pressionada para socar, inicia a animação Slash e para a Walk
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isPunching = !isPunching;
            animator.SetBool("MWalk", false);
            animator.SetBool("MSlash", isPunching);
        }

        // Verifica se o personagem está socando e se uma tecla de movimento foi pressionada
        if (isPunching && isWalking)
        {
            isPunching = false;
            animator.SetBool("MSlash", false);
            animator.SetBool("MWalk", true);
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

        float speed = isPunching ? punchSpeed : walkSpeed;

        rb.velocity = movement * speed + horizontalVelocity;

        // Verifica se a tecla "Q" foi solta para parar o soco, inicia a animação Walk e para a animação Slash
        if (Input.GetKeyUp(KeyCode.Q))
        {
            isPunching = false;
            animator.SetBool("MSlash", false);
            animator.SetBool("MWalk", true);
        }
    }
}
