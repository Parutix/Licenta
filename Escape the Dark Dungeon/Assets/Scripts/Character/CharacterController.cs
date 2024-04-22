using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f; // Adjust this value to control movement speed
    public float movementThreshold = 0.1f; // Adjust this value to set the threshold for transitioning between idle and run animations

    private void Update()
    {
        // Get horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Set horizontalMovement parameter in Animator
        animator.SetFloat("horizontalMovement", Mathf.Abs(horizontalInput));

        // Check if character is moving horizontally
        bool isMovingHorizontally = Mathf.Abs(horizontalInput) > movementThreshold;

        // Move the character
        Vector3 movement = new Vector3(horizontalInput, 0.0f, 0.0f) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Update animation state based on movement
        if (isMovingHorizontally)
        {
            // Transition to run animation
            animator.Play("Run");
        }
        else
        {
            // Transition to idle animation
            animator.Play("Idle");
        }
    }
}
