using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FifthNPCTalk : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private string[] dialogue;
    [SerializeField]
    private Text dialogueText;
    private int dialogueIndex = 0;
    [SerializeField]
    private float wordSpeed;
    private bool isPlayerInRange;
    private CharacterController playerController;
    void Start()
    {
        dialoguePanel.SetActive(false);
        playerController = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlayerInRange)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                if (dialogueText.text == dialogue[dialogueIndex])
                {
                    NextDialogue();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogue[dialogueIndex];
                }
            }
            else
            {
                playerController.enabled = false;
                dialoguePanel.SetActive(true);
                StartCoroutine(TypeText());
            }
        }
    }

    private void NextDialogue()
    {
        if (dialogueIndex < dialogue.Length - 1)
        {
            dialogueIndex++;
            dialogueText.text = "";
            StartCoroutine(TypeText());
        }
        else
        {
            playerController.enabled = true;
            resetDialogue();
        }
    }

    IEnumerator TypeText()
    {
        dialogueText.text = "";
        foreach (char letter in dialogue[dialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void resetDialogue()
    {
        dialogueIndex = 0;
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            resetDialogue();
        }
    }
}
