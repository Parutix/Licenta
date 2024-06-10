using System.Collections;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    [SerializeField]
    private float moveDistance = 2f;
    [SerializeField]
    private float moveDuration = 1f;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            yield return Move(Vector2.left);
            yield return new WaitForSeconds(15f);
            yield return Move(Vector2.right);
        }
    }

    IEnumerator Move(Vector2 direction)
    {
        Vector2 startPosition = rb.position;
        Vector2 targetPosition = startPosition + direction * moveDistance;
        float elapsedTime = 0;

        if (direction == Vector2.left && isFacingRight)
        {
            Flip();
        }
        else if (direction == Vector2.right && !isFacingRight)
        {
            Flip();
        }

        while (elapsedTime < moveDuration)
        {
            rb.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Update()
    {
    }
}
