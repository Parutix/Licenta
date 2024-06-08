using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Iceball : MonoBehaviour
{
    [SerializeField]
    private int damage = 35;
    [SerializeField]
    private float slowDuration = 3f;
    [SerializeField]
    private float slowAmount = 0.5f;
    [SerializeField]
    private Color iceballColor = new Color(0.75f, 0.75f, 1f);
    [SerializeField]
    private float colorDuration = 3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Iceball collided with " + collision.gameObject.name);
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Debug.Log("Enemy health found");
            enemyHealth.TakeDamage(damage);
            enemyHealth.ChangeColor(iceballColor, colorDuration);
            StartCoroutine(ApplySlowEffect(enemyHealth));
        }
        Destroy(gameObject);
    }

    private IEnumerator ApplySlowEffect(EnemyHealth enemyHealth)
    {
        NavMeshAgent agent = enemyHealth.GetComponentInChildren<NavMeshAgent>();
        if (agent != null)
        {
            float originalSpeed = agent.speed;
            agent.speed *= slowAmount;
            yield return new WaitForSeconds(slowDuration);
            agent.speed = originalSpeed;
        }
    }
}
