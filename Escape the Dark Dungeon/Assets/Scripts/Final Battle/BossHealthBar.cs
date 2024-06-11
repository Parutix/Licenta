using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1500;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private Image healthBar;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        float imageFill = (float)currentHealth / maxHealth;
        healthBar.fillAmount = imageFill;
    }

    public void BossTakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }
}
