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
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if(characterController == null)
        {
            Debug.LogError("CharacterController not found");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(characterController.activeSpell == 1)
            {
                characterController.activeSpell = 2;
            }
            else
            {
                characterController.activeSpell = 1;
            }
        }
    }
}
