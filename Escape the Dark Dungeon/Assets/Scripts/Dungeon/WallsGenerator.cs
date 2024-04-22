using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallsGenerator
{
    public static void GenerateWalls(HashSet<Vector2Int> floor, TilemapVisualizer tilemap)
    {
        HashSet<Vector2Int> basicWalls = (HashSet<Vector2Int>)FindWallsInDirections(floor, Direction2D.directionsList);
        foreach(var pos in basicWalls)
        {
            tilemap.paintSingleWall(pos);
        }
    }

    private static object FindWallsInDirections(HashSet<Vector2Int> floor, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
        foreach (var pos in floor)
        {
            foreach (var dir in directionsList)
            {
                Vector2Int next = pos + dir;
                if (!floor.Contains(next))
                {
                    walls.Add(next);
                }
            }
        }
        return walls;
    }
}
