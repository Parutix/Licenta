using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Fireball collided with " + collision.gameObject.name);
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Debug.Log("Enemy health found");
            enemyHealth.TakeDamage();
        }

        // Destroy the fireball after collision with any object
        Destroy(gameObject);
    }
}
