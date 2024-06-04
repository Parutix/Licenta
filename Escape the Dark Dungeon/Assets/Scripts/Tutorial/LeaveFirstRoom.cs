using System.Collections;
using UnityEngine;

public class LeaveFirstRoom : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    private GameObject NPC;
    [SerializeField]
    private Transform leavePoint;
    [SerializeField]
    private float runSpeed = 5f; 
    [SerializeField]
    private float startDelay = 0f; 
    private bool isRunning = false;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private CharacterController playerController;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); 
        playerController = player.GetComponent<CharacterController>();
    }

    public void RunOut()
    {
        Flip();
        animator.SetBool("Run Right", true);
        isRunning = true;
        StartCoroutine(StartRunningAfterDelay());
    }

    IEnumerator StartRunningAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        Vector2 direction = (leavePoint.position - transform.position).normalized;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            Vector2 direction = (leavePoint.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * runSpeed, rb.velocity.y); 

            if (Vector2.Distance(transform.position, leavePoint.position) < 0.1f)
            {
                Destroy(NPC);
                playerController.enabled = true;
                rb.velocity = Vector2.zero;
                animator.SetBool("Run Right", false); 
                isRunning = false;
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
