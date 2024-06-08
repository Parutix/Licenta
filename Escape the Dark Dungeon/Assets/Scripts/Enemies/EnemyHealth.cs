using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth = 100;
    private EnemiesCount enemiesCount;
    private Renderer enemyRenderer;
    private Color originalColor;

    private Coroutine colorCoroutine;
    private Coroutine dotCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        enemiesCount = FindObjectOfType<EnemiesCount>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
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

    public void ChangeColor(Color color, float duration)
    {
        if (enemyRenderer != null)
        {
            if (colorCoroutine != null)
            {
                StopCoroutine(colorCoroutine);
            }
            colorCoroutine = StartCoroutine(ChangeColorRoutine(color, duration));
        }
    }

    private IEnumerator ChangeColorRoutine(Color color, float duration)
    {
        enemyRenderer.material.color = color;
        yield return new WaitForSeconds(duration);
        enemyRenderer.material.color = originalColor;
    }

    public void ApplyDamageOverTime(int dotDamage, float dotDuration, float dotInterval)
    {
        if (dotCoroutine != null)
        {
            StopCoroutine(dotCoroutine);
        }
        dotCoroutine = StartCoroutine(ApplyDamageOverTimeRoutine(dotDamage, dotDuration, dotInterval));
    }

    private IEnumerator ApplyDamageOverTimeRoutine(int dotDamage, float dotDuration, float dotInterval)
    {
        float elapsed = 0f;
        while (elapsed < dotDuration)
        {
            yield return new WaitForSeconds(dotInterval);
            TakeDamage(dotDamage);
            elapsed += dotInterval;
        }
    }

    void Update()
    {

    }
}
