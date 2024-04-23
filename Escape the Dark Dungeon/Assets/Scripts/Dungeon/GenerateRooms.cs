using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

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
        List<Vector2Int> xRoomCenter = new List<Vector2Int>();
        foreach(var room in listOfRooms)
        {
            xRoomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        // After we did that we will create corridors between the rooms
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        corridors = GenerateCorridors(xRoomCenter);
        roomFloor.UnionWith(corridors);

        tilemapVisualizer.paintTiles(roomFloor);
        WallsGenerator.GenerateWalls(roomFloor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> RandomWalk(List<BoundsInt> listOfRooms)
    {
        HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();
        for(int i = 0; i < listOfRooms.Count; i++)
        {
            var room = listOfRooms[i];
            var center = new Vector2Int(Mathf.RoundToInt(room.center.x), Mathf.RoundToInt(room.center.y));
            var getFloor = SimpleRandomWalk(randomWalkData, center);
            foreach(var pos in getFloor)
            {
                if(pos.x >= (room.xMin + spaceBetween) && pos.x <= (room.xMax - spaceBetween) && 
                    pos.y >= (room.yMin - spaceBetween) && pos.y <= (room.yMax - spaceBetween))
                {
                      roomFloor.Add(pos);
                }
            }
        }
        return roomFloor;
    }

    private HashSet<Vector2Int> GenerateCorridors(List<Vector2Int> xRoomCenter)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var curentRoom = xRoomCenter[UnityEngine.Random.Range(0, xRoomCenter.Count)];
        xRoomCenter.Remove(curentRoom);
        while(xRoomCenter.Count > 0)
        {
            var nextRoom = GetClosestRoom(curentRoom, xRoomCenter);
            xRoomCenter.Remove(nextRoom);
            HashSet<Vector2Int> corridor = GenerateCorridorBetween2Rooms(curentRoom, nextRoom);
            curentRoom = nextRoom;
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


    private Vector2Int GetClosestRoom(Vector2Int currRoom, List<Vector2Int> xRoomCenter)
    {
        Vector2Int closestRoom = new Vector2Int(0, 0);
        float minDistance = float.MaxValue;
        Vector2Int randomRoom = xRoomCenter[UnityEngine.Random.Range(0, xRoomCenter.Count)];
        // We check all the rooms to get the closest one to our currRoom
        foreach(var room in xRoomCenter)
        {
            float distance = Vector2Int.Distance(currRoom, room);
            if(distance < minDistance)
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
}
