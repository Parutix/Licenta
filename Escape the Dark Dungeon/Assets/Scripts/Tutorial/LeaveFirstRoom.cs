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
    [SerializeField]
    private GuideText guideText;
    [SerializeField]
    private GameObject secondSpawn;
    [SerializeField]
    private Sprite sprite;

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
                guideText.setMoveText();
                rb.velocity = Vector2.zero;
                animator.SetBool("Run Right", false); 
                isRunning = false;
                GameObject secondNPC = Instantiate(NPC, secondSpawn.transform.position, Quaternion.identity);
                SpriteRenderer spriteRenderer = secondNPC.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                    spriteRenderer.sprite = sprite;
                }

                SecondNPCTalk secondNPCTalk = secondNPC.GetComponent<SecondNPCTalk>();
                if(secondNPCTalk != null)
                {
                    secondNPCTalk.enabled = true;
                }

                Animator secondAnimator = secondNPC.GetComponent<Animator>();
                if(secondAnimator != null)
                {
                    secondAnimator.enabled = true;
                }

                Rigidbody2D secondRigidbody = secondNPC.GetComponent<Rigidbody2D>();
                if (secondRigidbody != null)
                {
                    secondRigidbody.simulated = true;
                }

                CapsuleCollider2D secondCapsuleCollider = secondNPC.GetComponent<CapsuleCollider2D>();
                if (secondCapsuleCollider != null)
                {
                    secondCapsuleCollider.enabled = true;
                }

                BoxCollider2D secondBoxCollider = secondNPC.GetComponent<BoxCollider2D>();
                if (secondBoxCollider != null)
                {
                    secondBoxCollider.enabled = true;
                }

                SecondRoomTrial secondRoomTrial = secondNPC.GetComponent<SecondRoomTrial>();  
                if(secondRoomTrial != null)
                {
                    secondRoomTrial.enabled = true;
                }
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
