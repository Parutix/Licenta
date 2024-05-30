using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAttack : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField]
    private Image attackImage;
    [SerializeField]
    private TextMeshProUGUI attackText;
    [SerializeField]
    private Sprite fireballImage, iceballImage;
    private float cooldownTime = 4.0f;
    private float nextChangeTime = 0.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController not found");
        }
        attackText.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleAttackChange();
        HandleAttackCooldown();
    }

    private void HandleAttackChange()
    {
        if (Time.time >= nextChangeTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (characterController.activeSpell == 1)
                {
                    characterController.activeSpell = 2;
                    attackImage.sprite = fireballImage;
                }
                else
                {
                    characterController.activeSpell = 1;
                    attackImage.sprite = iceballImage;
                }
                nextChangeTime = Time.time + cooldownTime;
            }
        }
    }

    private void HandleAttackCooldown()
    {
        if (Time.time >= nextChangeTime)
        {
            attackText.gameObject.SetActive(false);
            attackImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            float timeLeft = Mathf.Ceil(nextChangeTime - Time.time);
            attackImage.color = new Color(1, 1, 1, 0.5f);
            attackText.text = $"{timeLeft:F0}s";
            attackText.gameObject.SetActive(true);
        }
    }
}
