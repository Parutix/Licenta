using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CorridorFirstDungeonGenerator : DungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercentage = 0.8f;
    protected override void RunMapGeneration()
    {
        FirstCorridorGeneration();
    }

    private void FirstCorridorGeneration()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        CreateCorridors(floorPos, potentialRoomPositions);
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
        
        // Logic for Blocked Corridors
        List<Vector2Int> blockedCorridors = FindBlockedCorridors(floorPos);
        SpawnRoomAtCorridorEnd(blockedCorridors, roomPositions);

        floorPos.UnionWith(roomPositions);
        tilemapVisualizer.paintTiles(floorPos);
        WallsGenerator.GenerateWalls(floorPos, tilemapVisualizer);
    }

    private void SpawnRoomAtCorridorEnd(List<Vector2Int> blockedCorridors, HashSet<Vector2Int> roomPositions)
    {
        foreach(var blockedCorridor in blockedCorridors)
        {
            if(roomPositions.Contains(blockedCorridor) == false)
            {
                var roomFloor = SimpleRandomWalk(randomWalkData, blockedCorridor);
                roomPositions.UnionWith(roomFloor);
            }
        }
    }

    private List<Vector2Int> FindBlockedCorridors(HashSet<Vector2Int> floorPos)
    {
        List<Vector2Int> blockedCorridors = new List<Vector2Int>();
        foreach (var pos in floorPos)
        {
            int cnt = 0;
            foreach(var side in Direction2D.directionsList)
            {
                if (floorPos.Contains(pos + side))
                {
                    cnt++;
                }
            }
            if(cnt == 1)
            {
                // We have found a blocked corridor
                blockedCorridors.Add(pos);
            }
        }
        return blockedCorridors;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercentage);
        List<Vector2Int> roomToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        foreach (var roomPosition in roomToCreate)
        {
            var roomFloor = SimpleRandomWalk(randomWalkData, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> potentialRoomPositions)
    {
        var current = startPosition;
        potentialRoomPositions.Add(current);
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = PGAlgorithm.RandomWalkCorridor(current, corridorLength);

            foreach (var tile in corridor)
            {
                var offsetTile1 = tile + Vector2Int.right;
                var offsetTile2 = tile + Vector2Int.up;
                floorPos.Add(tile);
                floorPos.Add(offsetTile1);
                floorPos.Add(offsetTile2);
                floorPos.Add(offsetTile1 + Vector2Int.up);
                floorPos.Add(offsetTile2 + Vector2Int.right);
            }

            current = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(current);
        }
    }


}
