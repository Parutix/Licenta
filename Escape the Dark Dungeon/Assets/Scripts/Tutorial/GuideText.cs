using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuideText : MonoBehaviour
{
    [SerializeField]
    private Text guideText;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void setMoveText()
    {
        guideText.text = "Move using WASD / Arrow Keys";
        StartCoroutine(waitForMovePress());
    }

    public void setDialogueText()
    {
        guideText.text = "Press Space to interact wtih characters";
        StartCoroutine(waitForSpacePress());
    }

    public void setDashText()
    {
        guideText.text = "Press Shift to dash";
        StartCoroutine(waitForDashPress());
    }

    public void setHealText()
    {
        guideText.text = "Press E to heal";
        StartCoroutine(waitForHealPress());
    }

    public void setAttackText()
    {
        guideText.text = "Press Q to change to Iceball";
        StartCoroutine(waitForAttackPress());
    }

    private IEnumerator waitForAttackPress()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
        guideText.text = "";
    }

    private IEnumerator waitForSpacePress()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        guideText.text = "";
    }

    private IEnumerator waitForMovePress()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) 
        || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) 
        || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow));
        guideText.text = "";
    }

    private IEnumerator waitForDashPress()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.LeftShift));
        guideText.text = "";
    }

    private IEnumerator waitForHealPress()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        guideText.text = "";
    }
}
