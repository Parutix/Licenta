using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondRoomTrial : MonoBehaviour
{
    [SerializeField]
    private GuideText guideText;
    private ThirdNPCTalk thirdNPCTalk;
    private SecondNPCTalk secondNPCTalk;

    private bool isDashTextCleared = false;

    void Start()
    {
        thirdNPCTalk = GetComponent<ThirdNPCTalk>();
        secondNPCTalk = GetComponent<SecondNPCTalk>();
    }

    void Update()
    {

    }

    public void LearnDash()
    {
        StartCoroutine(LearnDashCoroutine());
    }

    private IEnumerator LearnDashCoroutine()
    {
        guideText.setDashText();
        yield return new WaitUntil(() => IsDashTextCleared());
        StartCoroutine(LearnHealCoroutine());
    }

    private IEnumerator LearnHealCoroutine()
    {
        guideText.setHealText();
        yield return new WaitUntil(() => IsHealTextCleared());
        secondNPCTalk.enabled = false;
        thirdNPCTalk.enabled = true;
    }

    private bool IsDashTextCleared()
    {
        return guideText.GetComponentInChildren<Text>().text == "";
    }

    private bool IsHealTextCleared()
    {
        return guideText.GetComponentInChildren<Text>().text == "";
    }
}
