using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealthBar : MonoBehaviour
{
    public int maxHealth = 2500;
    public int currentHealth;
    public Image healthBar;
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
            SceneManager.LoadScene(7);
        }
    }
}
