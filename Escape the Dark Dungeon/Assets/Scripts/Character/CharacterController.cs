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
    public int activeSpell = 1;
    public GameObject fireballPrefab, iceballPrefab;
    public float fireballSpeed = 7f;
    public float iceballSpeed = 3.5f;

    private void OnEnable()
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
    }

    private void CharacterAttack(InputAction.CallbackContext context)
    {
        // Instantiate and launch the fireball
        if(activeSpell == 1)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb2 = fireball.GetComponent<Rigidbody2D>();
            Vector2 direction = pointerInput - (Vector2)transform.position;
            rb2.velocity = direction.normalized * fireballSpeed;
        }
        // Instantiate and launch the iceball
        else
        {
            GameObject iceball = Instantiate(iceballPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb2 = iceball.GetComponent<Rigidbody2D>();
            Vector2 direction = pointerInput - (Vector2)transform.position;
            rb2.velocity = direction.normalized * iceballSpeed;
        }
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
