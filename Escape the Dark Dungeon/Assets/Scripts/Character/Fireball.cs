using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private int damage = 20;
    [SerializeField]
    private int dotDamage = 15;
    [SerializeField]
    private float dotDuration = 3f;
    [SerializeField]
    private float dotInterval = 1f;
    [SerializeField]
    private Color fireballColor = new Color(1f, 0.75f, 0.75f);
    [SerializeField]
    private float colorDuration = 3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Fireball collided with " + collision.gameObject.name);

        BossHealthBar bossHealth = collision.gameObject.GetComponent<BossHealthBar>();
        if (bossHealth != null)
        {
            Debug.Log("Boss health found");
            bossHealth.BossTakeDamage(damage);
        }
        else
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("Enemy health found");
                enemyHealth.TakeDamage(damage);
                enemyHealth.ChangeColor(fireballColor, colorDuration);
                enemyHealth.ApplyDamageOverTime(dotDamage, dotDuration, dotInterval);
            }
        }
        Destroy(gameObject);
    }
}
