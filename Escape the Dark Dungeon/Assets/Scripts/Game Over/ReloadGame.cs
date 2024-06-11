using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReloadGame : MonoBehaviour
{
    [SerializeField]
    private Button tryAgainButton;
    void Start()
    {
        tryAgainButton.GetComponent<Button>();
        if (tryAgainButton == null)
        {
            Debug.LogError("Button is null");
        }
        tryAgainButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(3);
        });
    }

    void Update()
    {
        
    }
}
