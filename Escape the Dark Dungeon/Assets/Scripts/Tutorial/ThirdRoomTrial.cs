using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdRoomTrial : MonoBehaviour
{
    [SerializeField]
    private GuideText guideText;
    private FourthNPCTalk fourthNPCTalk;
    private FifthNPCTalk fifthNPCTalk;

    private void OnEnable()
    {
        DestroyBarrel.BarrelDestroyed += OnBarrelDestroyed;
    }

    private void OnDisable()
    {
        DestroyBarrel.BarrelDestroyed -= OnBarrelDestroyed;
    }

    void Start()
    {
        fourthNPCTalk = GetComponent<FourthNPCTalk>();
        fifthNPCTalk = GetComponent<FifthNPCTalk>();
    }

    void Update()
    {

    }

    public void LearnAttack()
    {
        StartCoroutine(LearnAttackCoroutine());
    }

    private IEnumerator LearnAttackCoroutine()
    {
        guideText.setAttackText();
        yield return new WaitUntil(() => IsAttackTextCleared());
        fourthNPCTalk.enabled = false;
        fifthNPCTalk.enabled = true;
        ThirdRoomTrial thirdRoomTrial = GetComponent<ThirdRoomTrial>();
        thirdRoomTrial.enabled = false;
    }

    private bool IsAttackTextCleared()
    {
        return guideText.GetComponentInChildren<Text>().text == "";
    }

    private void OnBarrelDestroyed()
    {
        LearnAttack();
    }
}
