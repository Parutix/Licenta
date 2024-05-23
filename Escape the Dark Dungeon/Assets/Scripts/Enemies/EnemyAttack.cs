using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private int enemyDamage = 3;
    private HealthBar playerHealth;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthBar>();
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component not found on the Player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found. Ensure it is named 'Player'.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
    }
}
