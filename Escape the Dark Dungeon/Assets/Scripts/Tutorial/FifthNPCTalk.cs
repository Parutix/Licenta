using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FifthNPCTalk : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    private GameObject dialoguePanel;
    private string[] dialogue;
    [SerializeField]
    private Text dialogueText;
    private int dialogueIndex = 0;
    [SerializeField]
    private float wordSpeed;
    private bool isPlayerInRange;
    private CharacterController playerController;
    private APIChatGPT apiChatGPT;
    private bool dialogueLoaded = false;
    void Start()
    {
        dialoguePanel.SetActive(false);
        playerController = player.GetComponent<CharacterController>();
        apiChatGPT = GetComponent<APIChatGPT>();
        if (apiChatGPT == null)
        {
            Debug.LogError("APIChatGPT component not found on the GameObject");
        }
        StartCoroutine(GenerateDialogue());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlayerInRange && dialogueLoaded)
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
        dialogueLoaded = false;
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

    private IEnumerator GenerateDialogue()
    {
        Debug.Log("GenerateDialogue coroutine started");
        yield return apiChatGPT.CompleteDialogue("Generate dialogue for an NPC who is a knight. " +
            "NO QUOTATION MARKS. The dialogue should consist of 4 sentences, each ending with a period (.) and without any labels like NPC: or Helper:." +
            "The dialogue should be longer, around 25-30 words per sentence. Do not include quotation marks or numbers before the sentences." +
            "Separate each sentence with a period (.) without spaces or new lines between them." +
            "The wizard(player) managed to destroy the barrel with his ice magic and the knight is in awe and congratulates him" +
            "the knight tells the player to continue without him as the knight will remain here and look for more survivors." +
            "After he tells the wizard he hopes he will see him after they get out of here(not exactly like this) and that they will drink an ale" +
            "on the knight's treat (not exactly like this)" +
            ". Just to make it clear, no numbers before the sentence, NO QUOTATION MARKS, just the sentences separated by dot (.) . ", (completedDialogue) =>
            {
                Debug.Log("Dialogue received from API: " + completedDialogue);
                dialogue = completedDialogue.Split(new[] { '.', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
                                            .Select(sentence => sentence.Trim())
                                            .Where(sentence => !string.IsNullOrEmpty(sentence))
                                            .ToArray();
                dialogueLoaded = true;
            });
    }
}

