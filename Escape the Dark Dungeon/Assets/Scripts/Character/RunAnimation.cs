using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAnimation : MonoBehaviour
{
    public float frameRate = 0.5f;
    public Sprite[] frames;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = false; // Track if the animation is currently playing

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= frameRate)
            {
                timer -= frameRate;
                currentFrame = (currentFrame + 1) % frames.Length;
                spriteRenderer.sprite = frames[currentFrame];
            }
        }
    }

    public void StartAnimation()
    {
        isPlaying = true;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[currentFrame]; // Set initial sprite
    }

    public void StopAnimation()
    {
        isPlaying = false;
    }
}
