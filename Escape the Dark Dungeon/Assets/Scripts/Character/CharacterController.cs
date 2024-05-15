using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private SpriteRenderer spriteRenderer;
    private Vector2 pointerInput, movementInput;
    public Vector2 PointerInput => pointerInput;
    [SerializeField]
    private InputActionReference movementAction, pointerAction, attackAction;

    public GameObject fireball;
    public float fireballSpeed = 5f;

    private void onEnable()
    {
        attackAction.action.performed += CharacterAttack;
    }

    private void OnDisable()
    {
        attackAction.action.performed -= CharacterAttack;
    }


    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        pointerInput = getMousePosition();
        RotateCharacter();
        movementInput = movementAction.action.ReadValue<Vector2>();
        characterMovement.movementInput = movementInput;

        if(attackAction.action.triggered)
        {
            CharacterAttack(default);
        }
    }
    private void CharacterAttack(InputAction.CallbackContext context)
    {
        GameObject fireball = Instantiate(this.fireball, transform.position, Quaternion.identity);
        Rigidbody2D rb2 = fireball.GetComponent<Rigidbody2D>();
        Vector2 direction = pointerInput - (Vector2)transform.position;
        rb2.velocity = direction.normalized * fireballSpeed;
    }

    private Vector2 getMousePosition()
    {
        Vector3 position = pointerAction.action.ReadValue<Vector2>();
        position.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(position);
    }

    private void RotateCharacter()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(
        mousePosition.x - transform.position.x,
        mousePosition.y - transform.position.y
        );
        transform.up = direction;
    }
}
