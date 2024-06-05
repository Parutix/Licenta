using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    [SerializeField]
    private float fixedVerticalSize = 5.0f;
    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
        SetCamera();
    }

    void Update()
    {
        if(Screen.width != 1920 || Screen.height != 1080)
        {
            Debug.LogError("Screen resolution is not 1920x1080");
        }
    }

    private void SetCamera()
    {
        if(cam.orthographic)
        {
            cam.orthographicSize = fixedVerticalSize;
        }
        else
        {
            Debug.LogError("Camera is not orthographic");
        }
    }
}
