using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateRooms : DungeonGenerator
{
    [SerializeField]
    private int roomWidth = 5;
    [SerializeField]
    private int roomLength = 5;
    [SerializeField]
    private int dungeonWidth = 20;
    [SerializeField]
    private int dungeonLength = 20;
    [SerializeField]
    [Range(0, 10)]
    private int spaceBetween = 1; // space between rooms
    [SerializeField]
    private bool randomWalk = false;
    [SerializeField]
    private GameObject monsterPrefab; // Monster prefab to spawn
    [SerializeField]
    private GameObject player; // Reference to the player GameObject
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TileBase[] validTile;

    protected override void RunMapGeneration()
    {
        RoomSpawner();
    }

    private void RoomSpawner()
    {
        var listOfRooms = PGAlgorithm.BinaryPartitioningAlgorithm(new BoundsInt((Vector3Int)startPosition,
            new Vector3Int(dungeonWidth, dungeonLength, 0)), roomLength, roomWidth);
        HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();

        if (randomWalk)
        {
            roomFloor = RandomWalk(listOfRooms);
        }
        else
        {
            roomFloor = CreateRooms(listOfRooms);
        }

        // Now we will find the center of each room and place it in a list
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in listOfRooms)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        // Find the player's starting room center
        Vector2Int playerStartRoomCenter = FindPlayerStartRoom(roomCenters);

        // Spawn monsters in each room except the player's start room
        SpawnMonstersInRooms(roomCenters, playerStartRoomCenter, roomFloor);

        // After we did that we will create corridors between the rooms
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        corridors = GenerateCorridors(roomCenters);
        roomFloor.UnionWith(corridors);

        tilemapVisualizer.paintTiles(roomFloor);
        WallsGenerator.GenerateWalls(roomFloor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> RandomWalk(List<BoundsInt> listOfRooms)
    {
        HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            var room = listOfRooms[i];
            var center = new Vector2Int(Mathf.RoundToInt(room.center.x), Mathf.RoundToInt(room.center.y));
            var getFloor = SimpleRandomWalk(randomWalkData, center);
            foreach (var pos in getFloor)
            {
                if (pos.x >= (room.xMin + spaceBetween) && pos.x <= (room.xMax - spaceBetween) &&
                    pos.y >= (room.yMin - spaceBetween) && pos.y <= (room.yMax - spaceBetween))
                {
                    roomFloor.Add(pos);
                }
            }
        }
        return roomFloor;
    }

    private HashSet<Vector2Int> GenerateCorridors(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoom = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoom);
        while (roomCenters.Count > 0)
        {
            var nextRoom = GetClosestRoom(currentRoom, roomCenters);
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
            corridor.Add(new Vector2Int(currRoom.x, currRoom.y + 1)); // Expand corridor one tile above
        }

        while (currRoom.y != nextRoom.y)
        {
            int step = (nextRoom.y > currRoom.y) ? 1 : -1;
            currRoom += new Vector2Int(0, step);
            corridor.Add(currRoom);
            corridor.Add(new Vector2Int(currRoom.x - 1, currRoom.y)); // Expand corridor one tile to the left
        }

        return corridor;
    }

    private Vector2Int GetClosestRoom(Vector2Int currRoom, List<Vector2Int> roomCenters)
    {
        Vector2Int closestRoom = new Vector2Int(0, 0);
        float minDistance = float.MaxValue;
        Vector2Int randomRoom = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        // We check all the rooms to get the closest one to our currRoom
        foreach (var room in roomCenters)
        {
            float distance = Vector2Int.Distance(currRoom, room);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRoom = room;
            }
        }
        // We return the closest room we found in the List
        return closestRoom;
    }

    private HashSet<Vector2Int> CreateRooms(List<BoundsInt> listOfRooms)
    {
        HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();
        foreach (var room in listOfRooms)
        {
            for (int x = spaceBetween; x < room.size.x - spaceBetween; x++)
            {
                for (int y = spaceBetween; y < room.size.y - spaceBetween; y++)
                {
                    Vector2Int temp = new Vector2Int(x, y) + (Vector2Int)room.min;
                    roomFloor.Add(temp);
                }
            }
        }
        return roomFloor;
    }

    private Vector2Int FindPlayerStartRoom(List<Vector2Int> roomCenters)
    {
        Vector2Int playerPosition = Vector2Int.RoundToInt(player.transform.position);
        Vector2Int closestRoomCenter = roomCenters[0];
        float minDistance = Vector2Int.Distance(playerPosition, closestRoomCenter);

        foreach (var roomCenter in roomCenters)
        {
            float distance = Vector2Int.Distance(playerPosition, roomCenter);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRoomCenter = roomCenter;
            }
        }

        return closestRoomCenter;
    }

    private void SpawnMonstersInRooms(List<Vector2Int> roomCenters, Vector2Int playerStartRoomCenter, HashSet<Vector2Int> roomFloor)
    {
        foreach (var roomCenter in roomCenters)
        {
            if (roomCenter != playerStartRoomCenter)
            {
                int monsterCount = UnityEngine.Random.Range(2, 4); // Spawns between 2 and 3 monsters
                for (int i = 0; i < monsterCount; i++)
                {
                    Vector2Int spawnPosition = GetValidSpawnPosition(roomCenter, roomFloor);
                    GameObject monster = Instantiate(monsterPrefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
                }
            }
        }
    }

    // Function so monsters don't spawn outside of the room
    private Vector2Int GetValidSpawnPosition(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor)
    {
        List<Vector2Int> validSpawnPositions = new List<Vector2Int>();

        for (int x = roomCenter.x - roomWidth / 2; x <= roomCenter.x + roomWidth / 2; x++)
        {
            for (int y = roomCenter.y - roomLength / 2; y <= roomCenter.y + roomLength / 2; y++)
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
            return roomCenter; // Fallback to room center if no valid positions are found
        }
    }
}
