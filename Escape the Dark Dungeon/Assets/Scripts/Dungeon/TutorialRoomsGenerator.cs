using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TutorialMapGenerator : DungeonGenerator
{
    [SerializeField]
    private int roomSize = 5;
    [SerializeField]
    private int spaceBetweenRooms = 2;
    [SerializeField]
    private GameObject monsterPrefab; // Monster prefab to spawn
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TileBase[] validTile;

    protected override void RunMapGeneration()
    {
        GenerateTutorialMap();
    }

    private void GenerateTutorialMap()
    {
        List<Vector2Int> roomCenters = new List<Vector2Int>();

        roomCenters.Add(new Vector2Int(0, 0)); // Room 1
        roomCenters.Add(new Vector2Int(roomSize + spaceBetweenRooms, 0)); // Room 2
        roomCenters.Add(new Vector2Int((roomSize + spaceBetweenRooms) * 2, 0)); // Room 3
        roomCenters.Add(new Vector2Int((roomSize + spaceBetweenRooms) * 3, 0)); // Room 4

        if (roomCenters.Count > 4)
        {
            roomCenters.Add(new Vector2Int(2 * (roomSize + spaceBetweenRooms), 0)); // Room 5 (optional)
        }

        HashSet<Vector2Int> roomFloor = CreateFixedRooms(roomCenters);

        Vector2Int playerStartRoomCenter = roomCenters[0];
        HashSet<Vector2Int> corridors = GenerateCorridors(roomCenters);
        roomFloor.UnionWith(corridors);

        tilemapVisualizer.paintTiles(roomFloor);
        WallsGenerator.GenerateWalls(roomFloor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateFixedRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();

        foreach (var center in roomCenters)
        {
            for (int x = -roomSize / 2; x <= roomSize / 2; x++)
            {
                for (int y = -roomSize / 2; y <= roomSize / 2; y++)
                {
                    roomFloor.Add(new Vector2Int(center.x + x, center.y + y));
                }
            }
        }

        return roomFloor;
    }

    private Vector2Int GetValidSpawnPosition(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor)
    {
        List<Vector2Int> validSpawnPositions = new List<Vector2Int>();

        for (int x = roomCenter.x - roomSize / 2; x <= roomCenter.x + roomSize / 2; x++)
        {
            for (int y = roomCenter.y - roomSize / 2; y <= roomCenter.y + roomSize / 2; y++)
            {
                Vector2Int potentialSpawn = new Vector2Int(x, y);
                if (roomFloor.Contains(potentialSpawn))
                {
                    validSpawnPositions.Add(potentialSpawn);
                }
            }
        }

        if (validSpawnPositions.Count > 0)
        {
            return validSpawnPositions[UnityEngine.Random.Range(0, validSpawnPositions.Count)];
        }
        else
        {
            return roomCenter;
        }
    }

    private HashSet<Vector2Int> GenerateCorridors(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        Vector2Int currentRoom = roomCenters[0];
        roomCenters.Remove(currentRoom);

        while (roomCenters.Count > 0)
        {
            Vector2Int nextRoom = GetClosestRoom(currentRoom, roomCenters);
            roomCenters.Remove(nextRoom);
            HashSet<Vector2Int> corridor = GenerateCorridorBetween2Rooms(currentRoom, nextRoom);
            currentRoom = nextRoom;
            corridors.UnionWith(corridor);
        }

        return corridors;
    }

    private HashSet<Vector2Int> GenerateCorridorBetween2Rooms(Vector2Int currRoom, Vector2Int nextRoom)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();

        corridor.Add(currRoom);

        while (currRoom.x != nextRoom.x)
        {
            int step = (nextRoom.x > currRoom.x) ? 1 : -1;
            currRoom += new Vector2Int(step, 0);
            corridor.Add(currRoom);
        }

        while (currRoom.y != nextRoom.y)
        {
            int step = (nextRoom.y > currRoom.y) ? 1 : -1;
            currRoom += new Vector2Int(0, step);
            corridor.Add(currRoom);
        }

        return corridor;
    }

    private Vector2Int GetClosestRoom(Vector2Int currRoom, List<Vector2Int> roomCenters)
    {
        Vector2Int closestRoom = roomCenters[0];
        float minDistance = Vector2Int.Distance(currRoom, closestRoom);

        foreach (var room in roomCenters)
        {
            float distance = Vector2Int.Distance(currRoom, room);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRoom = room;
            }
        }

        return closestRoom;
    }
}
