using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Transform player;
    public float fadeDuration = 2f;
    public float delay = 2f;
    private CharacterController playerController;
    private Image fadeImage;
    private Color targetColor;
    private float fadeSpeed;
    private bool isFading = false;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        targetColor = new Color(0, 0, 0, 0);
        fadeSpeed = 1f / fadeDuration;
        StartCoroutine(StartFadeAfterDelay());
        playerController = player.GetComponent<CharacterController>();
        playerController.enabled = false;
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    IEnumerator StartFadeAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        isFading = true;
    }

    void Update()
    {
        if (isFading)
        {
            fadeImage.color = Color.Lerp(fadeImage.color, targetColor, fadeSpeed * Time.deltaTime);

            if (Mathf.Approximately(fadeImage.color.a, 0f))
            {
                fadeImage.color = targetColor;
                isFading = false;
                gameObject.SetActive(false);
            }
        }
    }
}
