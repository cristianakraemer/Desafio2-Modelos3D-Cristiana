using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private bool isRunning = false;
    private Quaternion targetRotation;

    public float rotationSpeed = 6f;
    public float walkSpeed = 1.4f;
    public float runSpeed = 6f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation; // Inicializa a rota��o alvo como a rota��o atual
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Define a velocidade com base na caminhada ou corrida
        float speed = isRunning ? runSpeed : walkSpeed;

        // Altera os par�metros da anima��o
        animator.SetBool("Fwalk", movement != Vector3.zero != isRunning);
        animator.SetBool("FRun", isRunning);

        // Verifica se a tecla "E" foi pressionada para correr, inicia a anima��o Run e para a Walk
        if (Input.GetKeyDown(KeyCode.E))
        {
            isRunning = !isRunning;
            animator.SetBool("Fwalk", false);
            animator.SetBool("FRun", isRunning);
        }

        // Para girar o personagem suavemente
        if (movement != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(movement); // Calcula a rota��o alvo
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        // Aplica a movimenta��o usando o Rigidbody
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f; // Mant�m a velocidade vertical constante para evitar saltos indesejados

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        float speed = isRunning ? runSpeed : walkSpeed;

        rb.velocity = movement * speed + horizontalVelocity;

        // Verifica se a tecla "E" foi solta para parar de correr, inicia a anima��o Walk e para a anima��o Run
        if (Input.GetKeyUp(KeyCode.E))
        {
            isRunning = false;
            animator.SetBool("FRun", false);
            animator.SetBool("Fwalk", true);
        }
    }
}
