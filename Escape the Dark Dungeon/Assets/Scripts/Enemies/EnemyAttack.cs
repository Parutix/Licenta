using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private int enemyDamage = 3;
    private HealthBar playerHealth;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 prevPos;
    private float timeBetweenAttacks = 1f;
    private bool isFacingRight = true;
    private bool isAttacking = false;
    public bool IsAttacking => IsAttacking;
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthBar>();
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component not found.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found.");
        }

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
        }

    }

    private void Update()
    {
        if (navMeshAgent != null && animator != null)
        {
            Vector3 velocity = navMeshAgent.velocity;
            bool Run = velocity.magnitude > 0;
            animator.SetBool("Run", Run);

            if(Run)
            {
                if(velocity.x < 0 && isFacingRight)
                {
                    FlipPosition();
                }
                else if (velocity.x > 0 && !isFacingRight)
                {
                    FlipPosition();
                }
            }
            prevPos = transform.position;
        }
    }

    private void FlipPosition()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // First prototype of attack - on collision

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyDamage);
            }
            else
            {
                Debug.LogError("PlayerHealth reference is null.");
            }
        }
    }*/

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        while(isAttacking)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            ProcessAttack();
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    private void ProcessAttack()
    {
        playerHealth.TakeDamage(enemyDamage);
        animator.SetTrigger("Attack");
    }

    public void AttackPlayer()
    {
        if(!isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void OnDisable()
    {
        isAttacking = false;
    }


}
