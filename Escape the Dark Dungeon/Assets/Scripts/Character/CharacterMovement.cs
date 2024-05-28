using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField]
    private float speed, acceleration, deceleration;
    [SerializeField]
    private float dashSpeed, dashDuration, dashCooldown;
    [SerializeField]
    private float healCooldown;
    [SerializeField]
    private int healAmount;
    private float currSpeed = 0;
    private Vector2 movementBefore;
    private bool isDashing = false;
    private float dashTime = 0;
    private float dashCooldownTime = 0;
    private float healCooldownTime = 0;
    [SerializeField]
    private Image dashImage, healImage;
    [SerializeField]
    private TextMeshProUGUI dashCooldownText, healCooldownText;
    private HealthBar healthBar;

    public Vector2 movementInput { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogError("Rigidbody2D component not found");
        }

        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("Animator component not found");
        }

        healthBar = GetComponent<HealthBar>();
        if(healthBar == null)
        {
            Debug.LogError("HealthBar component not found");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleDash();
        HandleDashCooldown();
        HandleHeal();
        HandleHealCooldown();
    }

    void HandleMovement()
    {
        if (isDashing)
        {
            currSpeed = dashSpeed;
        }
        else if (movementInput.magnitude > 0 && currSpeed >= 0)
        {
            movementBefore = movementInput;
            currSpeed += Time.deltaTime * acceleration * speed;
        }
        else
        {
            currSpeed -= Time.deltaTime * speed * deceleration;
        }

        currSpeed = Mathf.Clamp(currSpeed, 0, isDashing ? dashSpeed : speed);
        rb.velocity = movementBefore * currSpeed;

        float movementSpeedNormalized = currSpeed / speed;
        animator.SetFloat("Movement", movementSpeedNormalized);
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTime <= 0 && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
                dashCooldownTime = dashCooldown;
            }
        }
        else if (dashCooldownTime > 0)
        {
            dashCooldownTime -= Time.deltaTime;
        }
    }

    private void HandleHeal()
    {
        if(Input.GetKeyDown(KeyCode.E) && healCooldownTime <= 0)
        {
            healthBar.Heal(healAmount);
            healCooldownTime = healCooldown;
        }

        if(healCooldownTime > 0)
        {
            healCooldownTime -= Time.deltaTime;
        }
    }

    private void HandleDashCooldown()
    {
        if(dashCooldownTime > 0)
        {
            dashImage.color = new Color(1, 1, 1, 0.5f);
            dashCooldownText.text = $"{dashCooldownTime:F0}";
            dashCooldownText.gameObject.SetActive(true);
        }
        else
        {
            dashImage.color = new Color(1, 1, 1, 1);
            dashCooldownText.gameObject.SetActive(false);
        }
    }

    private void HandleHealCooldown()
    {
        if(healCooldownTime > 0)
        {
            healImage.color = new Color(1, 1, 1, 0.5f);
            healCooldownText.text = $"{healCooldownTime:F0}";
            healCooldownText.gameObject.SetActive(true);
        }
        else
        {
            healImage.color = new Color(1, 1, 1, 1);
            healCooldownText.gameObject.SetActive(false);
        }
    }
}
