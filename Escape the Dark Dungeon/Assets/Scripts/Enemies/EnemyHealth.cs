using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHits = 3;
    private int currentHits = 0;
    private EnemiesCount enemiesCount;
    void Start()
    {
        currentHits = 0;
        enemiesCount = FindObjectOfType<EnemiesCount>();
    }

    public void TakeDamage()
    {
        currentHits++;
        if (currentHits >= maxHits)
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
