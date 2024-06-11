using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private Image healthBar;
    // private Color fullHealth = Color.green;
    // private Color midHealth = Color.yellow;
    // private Color lowHealth = Color.red;
    void Start()
    {
        if(currentHealth != maxHealth)
        {
            currentHealth = 60;
        }
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealth()
    {
        float imageFill = (float)currentHealth / maxHealth;
        healthBar.fillAmount = imageFill;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(6);
        }
        else
        {
            UpdateHealth();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealth();
    }
}
