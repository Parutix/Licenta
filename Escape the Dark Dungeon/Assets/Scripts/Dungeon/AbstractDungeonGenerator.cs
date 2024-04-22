using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = new Vector2Int(0, 0);

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        RunMapGeneration();
    }

    protected abstract void RunMapGeneration();
}
