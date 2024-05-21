using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHits = 3;
    private int currentHits = 0;

    void Start()
    {
        currentHits = 0;
    }

    public void TakeDamage()
    {
        currentHits++;
        if (currentHits >= maxHits)
        {
            Debug.Log("Enemy destroyed");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
