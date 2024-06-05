using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerHUD : MonoBehaviour
{
    [SerializeField]
    private GameObject health;
    [SerializeField]
    private GameObject dash;
    [SerializeField]
    private GameObject heal;
    [SerializeField]
    private GameObject attack;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void showHUD()
    {
        health.SetActive(true);
        dash.SetActive(true);
        heal.SetActive(true);
        attack.SetActive(true);
    }
}
