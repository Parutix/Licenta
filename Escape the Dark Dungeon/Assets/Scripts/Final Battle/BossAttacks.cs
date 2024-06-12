using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    [SerializeField] private GameObject firstProjectile;
    [SerializeField] private GameObject secondProjectile;
    [SerializeField] private GameObject thirdProjectile;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float attackInterval = 10f;
    [SerializeField] private float fastAttackInterval = 5f;
    private float currentAttackInterval;
    private HealthBar playerHealth;
    private float attackTimer;
    private int currentAttackIndex;
    private List<System.Action> attackPatterns;
    private BossHealthBar bossHealthBar;

    void Start()
    {
        attackTimer = attackInterval;
        currentAttackInterval = attackInterval;
        currentAttackIndex = 0;

        attackPatterns = new List<System.Action>
        {
            SpiralAttack,
            RadialBurstAttack,
            TargetedShotAttack
        };

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthBar>();
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component not found.");
            }
        }

        GameObject boss = GameObject.Find("Boss");
        if (boss != null)
        {
            bossHealthBar = boss.GetComponent<BossHealthBar>();
            if (bossHealthBar == null)
            {
                Debug.LogError("BossHealthBar component not found.");
            }
        }
    }

    void Update()
    {
        if (bossHealthBar != null)
        {
            if (bossHealthBar.currentHealth <= bossHealthBar.maxHealth / 2)
            {
                currentAttackInterval = fastAttackInterval;
            }
            else
            {
                currentAttackInterval = attackInterval;
            }
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            attackTimer = currentAttackInterval;
            PerformAttack();
            currentAttackIndex = (currentAttackIndex + 1) % attackPatterns.Count;
        }
    }

    void PerformAttack()
    {
        attackPatterns[currentAttackIndex].Invoke();
    }

    void SpiralAttack()
    {
        StartCoroutine(SpiralAttackRoutine());
    }

    IEnumerator SpiralAttackRoutine()
    {
        int numProjectiles = 30;
        float angleStep = 360f / numProjectiles;
        float currentAngle = 0f;

        for (int i = 0; i < numProjectiles; i++)
        {
            float projectileDirX = transform.position.x + Mathf.Sin((currentAngle * Mathf.PI) / 180);
            float projectileDirY = transform.position.y + Mathf.Cos((currentAngle * Mathf.PI) / 180);
            Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0f);
            Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized * projectileSpeed;

            GameObject proj = Instantiate(firstProjectile, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            currentAngle += angleStep;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void RadialBurstAttack()
    {
        int numProjectiles = 12;
        float angleStep = 360f / numProjectiles;

        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = i * angleStep;
            float projectileDirX = Mathf.Sin((angle * Mathf.PI) / 180);
            float projectileDirY = Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0f);
            Vector3 projectileMoveDirection = projectileVector.normalized * projectileSpeed;

            GameObject proj = Instantiate(secondProjectile, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
        }
    }

    void TargetedShotAttack()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized * projectileSpeed;
            GameObject proj = Instantiate(thirdProjectile, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
        }
    }
}
