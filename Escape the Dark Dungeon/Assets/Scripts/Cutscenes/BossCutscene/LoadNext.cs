using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNext : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene(5);
    }

}
