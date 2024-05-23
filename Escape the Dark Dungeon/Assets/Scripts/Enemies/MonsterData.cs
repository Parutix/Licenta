using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterData
{
    private static int _monsterCounter = 1;
    public static int MonsterCounter
    {
        get { return _monsterCounter; }
        set
        {
            _monsterCounter = value;
        }
    }
}
