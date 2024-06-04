using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCharacter : MonoBehaviour
{
    public Transform player;
    public float runSpeed = 5f; 
    public float startDelay = 6f;
    private bool isRunning = false;
    private Animator animator;
    private Rigidbody2D rb;
    private CharacterController playerController;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<CharacterController>();
        animator.SetBool("Run", true);
        Invoke("StartRunning", startDelay);
    }

    void StartRunning()
    {
        isRunning = true;

        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    void Update()
    {
        if (isRunning)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * runSpeed, rb.velocity.y);

            if (Vector2.Distance(transform.position, player.position) < 2f)
            {
                rb.velocity = Vector2.zero;
                isRunning = false;
                animator.SetBool("Run", false);
                playerController.enabled = false;
            }
        }
    }
}
