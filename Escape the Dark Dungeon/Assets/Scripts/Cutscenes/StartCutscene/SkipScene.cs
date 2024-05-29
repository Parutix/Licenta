using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipScene : MonoBehaviour
{
    [SerializeField]
    private Button skipButton;
    void Start()
    {
        skipButton.GetComponent<Button>();
        if(skipButton == null)
        {
            Debug.LogError("Skip Button is null");
        }
        skipButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(2);
        });
    }

    void Update()
    {
        
    }
}
