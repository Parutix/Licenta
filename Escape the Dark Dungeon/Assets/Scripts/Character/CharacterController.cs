using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private Vector2 pointerInput, movementInput;
    public Vector2 PointerInput => pointerInput;
    [SerializeField]
    private InputActionReference movementAction, pointerAction, attackAction;

    private void onEnable()
    {
        attackAction.action.performed += CharacterAttack;
    }

    private void OnDisable()
    {
        attackAction.action.performed -= CharacterAttack;
    }

    private void CharacterAttack(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }
    private void Update()
    {
        pointerInput = getMousePosition();
        movementInput = movementAction.action.ReadValue<Vector2>();
        characterMovement.movementInput = movementInput;

        if(attackAction.action.triggered)
        {
            Debug.Log("Attack");
        }
    }

    private Vector2 getMousePosition()
    {
        Vector3 position = pointerAction.action.ReadValue<Vector2>();
        position.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(position);
    }
}
