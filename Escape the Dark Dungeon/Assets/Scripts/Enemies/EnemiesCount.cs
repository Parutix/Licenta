using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemiesCount : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countEnemies;
    private int killedEnemies = 0;
    void Start()
    {
        countEnemies.text = "0 / " + MonsterData.MonsterCounter + " Monsters Killed";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateKilledEnemies()
    {
        killedEnemies++;
        countEnemies.text = killedEnemies + " / " + MonsterData.MonsterCounter + " Monsters Killed";
    }
}
