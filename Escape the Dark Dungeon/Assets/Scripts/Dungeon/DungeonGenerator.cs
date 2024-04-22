using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected SRWData randomWalkData;
        
    protected override void RunMapGeneration()
    {
        HashSet<Vector2Int> floorPos = SimpleRandomWalk(randomWalkData, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.paintTiles(floorPos);
        WallsGenerator.GenerateWalls(floorPos, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> SimpleRandomWalk(SRWData walkParams, Vector2Int pos)
    {
        var current = pos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for(int i = 0; i < walkParams.noOfIterations; i++)
        {
            HashSet<Vector2Int> path = PGAlgorithm.SimpleRandomWalk(current, walkParams.walkLength);
            floorPositions.UnionWith(path);
            if(walkParams.startRandom)
            {
                current = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
