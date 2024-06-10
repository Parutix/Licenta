using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBossProjectile : MonoBehaviour
{
    public LayerMask projectileLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Boss projectile collided with " + collision.gameObject.name);

        if (collision.gameObject == gameObject)
        {
            return;
        }

        if (collision.gameObject.name == "fireball-red-tail-big(Clone)")
        {
            return;
        }

        if (collision.gameObject.name == "Boss")
        {
            return;
        }

        Destroy(gameObject);
    }
}
