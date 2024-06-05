using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveSecondRoom : MonoBehaviour
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
    private GameObject thirdSpawn;
    [SerializeField]
    private Sprite sprite;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<CharacterController>();
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
                GameObject thirdNPC = Instantiate(NPC, thirdSpawn.transform.position, Quaternion.identity);
                SpriteRenderer spriteRenderer = thirdNPC.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = sprite;
                }
                FourthNPCTalk fourthNPCTalk = thirdNPC.GetComponent<FourthNPCTalk>();
                if (fourthNPCTalk != null)
                {
                    fourthNPCTalk.enabled = true;
                }

                Rigidbody2D thirdRigidbody = thirdNPC.GetComponent<Rigidbody2D>();
                if (thirdRigidbody != null)
                {
                    thirdRigidbody.simulated = true;
                }

                CapsuleCollider2D thirdCapsuleCollider = thirdNPC.GetComponent<CapsuleCollider2D>();
                if (thirdCapsuleCollider != null)
                {
                    thirdCapsuleCollider.enabled = true;
                }

                Animator thirdAnimator = thirdNPC.GetComponent<Animator>();
                if (thirdAnimator != null)
                {
                    thirdAnimator.enabled = true;
                }

                BoxCollider2D thirdBoxCollider = thirdNPC.GetComponent<BoxCollider2D>();
                if (thirdBoxCollider != null)
                {
                    thirdBoxCollider.enabled = true;
                }

                SecondRoomTrial secondRoomTrial = thirdNPC.GetComponent<SecondRoomTrial>();
                if (secondRoomTrial != null)
                {
                    secondRoomTrial.enabled = true;
                }
            }
        }
    }

    public void RunOutRoomTwo()
    {
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

}
