using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossRoomGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    private Vector2Int bossRoomSize = new Vector2Int(30, 30); // Size of the boss room
    [SerializeField]
    private Vector2Int corridorSize = new Vector2Int(10, 3); // Length and width of the corridor
    [SerializeField]
    private Vector2Int playerRoomSize = new Vector2Int(6, 6); // Size of the player's room
    [SerializeField]
    private Vector2Int[] pillarPositions; // Positions where pillars will be placed
    [SerializeField]
    private Vector2Int pillarSize = new Vector2Int(2, 2); // Size of each pillar

    protected override void RunMapGeneration()
    {
        tilemapVisualizer.Clear();

        // Generate the boss room
        HashSet<Vector2Int> bossRoomPositions = GenerateSquareRoom(startPosition, bossRoomSize);
        tilemapVisualizer.paintTiles(bossRoomPositions);
        GeneratePillars(bossRoomPositions);

        // Generate the corridor
        Vector2Int corridorStartPos = new Vector2Int(startPosition.x + bossRoomSize.x / 2 - corridorSize.x / 2, startPosition.y - corridorSize.y);
        HashSet<Vector2Int> corridorPositions = GenerateCorridor(corridorStartPos, corridorSize);
        tilemapVisualizer.paintTiles(corridorPositions);

        // Generate the player spawn room
        Vector2Int playerRoomStartPos = new Vector2Int(corridorStartPos.x + corridorSize.x / 2 - playerRoomSize.x / 2, corridorStartPos.y - playerRoomSize.y);
        HashSet<Vector2Int> playerRoomPositions = GenerateSquareRoom(playerRoomStartPos, playerRoomSize);
        tilemapVisualizer.paintTiles(playerRoomPositions);

        // Generate walls for the entire dungeon
        HashSet<Vector2Int> allFloorPositions = new HashSet<Vector2Int>();
        allFloorPositions.UnionWith(bossRoomPositions);
        allFloorPositions.UnionWith(corridorPositions);
        allFloorPositions.UnionWith(playerRoomPositions);
        WallsGenerator.GenerateWalls(allFloorPositions, tilemapVisualizer);
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

    private HashSet<Vector2Int> GenerateCorridor(Vector2Int startPosition, Vector2Int corridorSize)
    {
        HashSet<Vector2Int> corridorPositions = new HashSet<Vector2Int>();
        for (int x = 0; x < corridorSize.x; x++)
        {
            for (int y = 0; y < corridorSize.y; y++)
            {
                corridorPositions.Add(new Vector2Int(startPosition.x + x, startPosition.y - y));
            }
        }
        return corridorPositions;
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
