using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemiesCount : MonoBehaviour
{
    [SerializeField]
    private Text countEnemies;
    [SerializeField]
    private GameFadeOut fadeOut;
    private int killedEnemies = 0;
    private int totalMonsters = 0;

    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.ToLower().Contains("monster"))
            {
                totalMonsters++;
            }
        }

        countEnemies.text = killedEnemies + " / " + totalMonsters + " Monsters Killed";
    }

    void Update()
    {
    }

    public void UpdateKilledEnemies()
    {
        killedEnemies++;
        countEnemies.text = killedEnemies + " / " + totalMonsters + " Monsters Killed";
        if(killedEnemies == totalMonsters)
        {
            StartCoroutine(HandleFinishDungeon());
        }
    }

    private IEnumerator HandleFinishDungeon()
    {
        fadeOut.StartCoroutine(fadeOut.FadeOut());  
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(4);
    }
}
