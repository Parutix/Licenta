using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossRoomGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    private Vector2Int roomSize = new Vector2Int(30, 30);
    [SerializeField]
    private Vector2Int[] pillarPositions;
    [SerializeField]
    private Vector2Int pillarSize = new Vector2Int(2, 2);

    protected override void RunMapGeneration()
    {
        HashSet<Vector2Int> floorPos = GenerateSquareRoom(startPosition, roomSize);
        tilemapVisualizer.Clear();
        tilemapVisualizer.paintTiles(floorPos);
        WallsGenerator.GenerateWalls(floorPos, tilemapVisualizer);
        GeneratePillars(floorPos);
    }

    private HashSet<Vector2Int> GenerateSquareRoom(Vector2Int startPosition, Vector2Int roomSize)
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int x = 0; x < roomSize.x; x++)
        {
            for (int y = 0; y < roomSize.y; y++)
            {
                floorPositions.Add(new Vector2Int(startPosition.x + x, startPosition.y + y));
            }
        }
        return floorPositions;
    }

    private void GeneratePillars(HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int pos in pillarPositions)
        {
            for (int x = 0; x < pillarSize.x; x++)
            {
                for (int y = 0; y < pillarSize.y; y++)
                {
                    Vector2Int pillarTile = pos + new Vector2Int(x, y);
                    if (floorPositions.Contains(pillarTile))
                    {
                        floorPositions.Remove(pillarTile);
                        tilemapVisualizer.paintSingleWall(pillarTile);
                    }
                }
            }
        }
    }
}
