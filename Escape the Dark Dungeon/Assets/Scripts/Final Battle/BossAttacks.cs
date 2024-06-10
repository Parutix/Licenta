using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float attackInterval = 15f;

    private float attackTimer;
    private int currentAttackIndex;
    private List<System.Action> attackPatterns;

    void Start()
    {
        attackTimer = attackInterval;
        currentAttackIndex = 0;

        attackPatterns = new List<System.Action>
        {
            SpiralAttack,
            RadialBurstAttack,
            TargetedShotAttack
        };
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            attackTimer = attackInterval;
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

            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
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

            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
        }
    }

    void TargetedShotAttack()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized * projectileSpeed;
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
        }
    }
}
