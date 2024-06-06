using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class NPCTalk : MonoBehaviour
{
    public Transform player;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    private int dialogueIndex = 0;
    [SerializeField] private float wordSpeed;
    private bool isPlayerInRange;
    [SerializeField] private LeaveFirstRoom leaveFirstRoom;
    private CharacterController playerController;
    private APIChatGPT apiChatGPT;
    private string[] dialogue;
    private bool dialogueLoaded = false;

    private void Start()
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

    private void Update()
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
        dialogueIndex++;
        if (dialogueIndex < dialogue.Length)
        {
            StartCoroutine(TypeText());
        }
        else
        {
            ResetDialogue();
            leaveFirstRoom.RunOut();
        }
    }

    private IEnumerator TypeText()
    {
        Debug.Log("TypeText coroutine started");
        dialogueText.text = "";
        if (!string.IsNullOrEmpty(dialogue[dialogueIndex]))
        {
            foreach (char letter in dialogue[dialogueIndex].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordSpeed);
            }
        }
        Debug.Log("Finished typing text: " + dialogueText.text);
    }

    private void ResetDialogue()
    {
        dialogue = null;
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        dialogueLoaded = false;
        dialogueIndex = 0; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player exited range");
            ResetDialogue();
        }
    }

     private IEnumerator GenerateDialogue()
     {
        Debug.Log("GenerateDialogue coroutine started");
        yield return apiChatGPT.CompleteDialogue("Generate dialogue for an NPC who is a knight. The dialogue should consist of 4 sentences, " +
            "each ending with a period (.) and without any labels like NPC: or Helper:. NO QUOTATION MARKS. The knight believes he was the last one left alive and " +
            "only recognizes the player as a powerful mage after speaking. The dialogue should be longer, around 25-30 words per sentence." +
            "The knight should say something like, \"Thank God I thought I was the only one left alive.\" but not exactly that " +
            "The knight should recognize the player as the best mage in the kingdom and tell the player to follow him because he will help them escape." +
            "The knight does not have any weapons" +
            "Do not include quotation marks or numbers before the sentences." +
            "Separate each sentence with a period (.) without spaces or new lines between them." +
            "Ensure the dialogue reflects that the knight initially doesn't recognize the player but does so after a moment. " +
            "The knight's speech should be clear and coherent, focusing on expressing relief, recognition, and a plan to help the player escape.", (completedDialogue) =>
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
