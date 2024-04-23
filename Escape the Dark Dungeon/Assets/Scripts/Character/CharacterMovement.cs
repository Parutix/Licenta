using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField]
    private float speed, acceleration, deceleration;
    [SerializeField]
    private float currSpeed = 0;
    private Vector2 movementBefore;
    public Vector2 movementInput { get; set; }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(movementInput.magnitude > 0 && currSpeed >= 0)
        {
            movementBefore = movementInput;
            currSpeed += Time.deltaTime * acceleration * speed;
        }
        else
        {
            currSpeed -= Time.deltaTime * speed * deceleration;
        }
        currSpeed = Mathf.Clamp(currSpeed, 0, speed);
        rb.velocity = movementBefore * currSpeed;

        float movementSpeedNormalized = currSpeed / speed;
        animator.SetFloat("Movement", movementSpeedNormalized);
    }
}
