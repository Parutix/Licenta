using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;
    public TextMeshProUGUI health;
    void Start()
    {
        currentHealth = maxHealth;
        health.text = "Health: " + currentHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(4);
        }
        health.text = "Health: " + currentHealth;
        if (currentHealth <= 0)
        {
            Debug.Log("Player destroyed");
            Destroy(gameObject);
        }
    }
}
