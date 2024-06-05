using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeAndLoadScene : MonoBehaviour
{
    public float fadeDuration = 1f;
    private bool fadeOut = false;
    [SerializeField]
    private Image blackImage;

    private void Start()
    {

    }

    private void Update()
    {
        if (fadeOut)
        {
            FadeToBlack();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fadeOut = true;
        }
    }

    private void FadeToBlack()
    {
        float fadeSpeed = 1f / fadeDuration;
        Color currentColor = blackImage.color;
        currentColor.a += fadeSpeed * Time.deltaTime;
        blackImage.color = currentColor;
        if (currentColor.a >= 1f)
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(3);
    }
}
