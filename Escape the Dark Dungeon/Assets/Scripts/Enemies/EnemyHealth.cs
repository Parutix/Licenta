using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth = 100;
    private EnemiesCount enemiesCount;
    void Start()
    {
        currentHealth = maxHealth;
        enemiesCount = FindObjectOfType<EnemiesCount>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            enemiesCount.UpdateKilledEnemies();
            Debug.Log("Enemy destroyed");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
