using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrel : MonoBehaviour
{
    public static event Action BarrelDestroyed;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "fireball-blue-big(Clone)")
        {
            Destroy(gameObject);
            BarrelDestroyed?.Invoke();
        }
    }
}
