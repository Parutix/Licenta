using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class PGAlgorithm
{
    private static void shuffleSplit<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static List<BoundsInt> BinaryPartitioningAlgorithm(BoundsInt bounds, int minRoomLength, int minRoomWidth)
    {
        Queue<BoundsInt> queueOfRooms = new Queue<BoundsInt>();
        List<BoundsInt> listOfRooms = new List<BoundsInt>();
        queueOfRooms.Enqueue(bounds);
        while (queueOfRooms.Count > 0)
        {
            var currentRoom = queueOfRooms.Dequeue();
            if(currentRoom.size.x > minRoomWidth && currentRoom.size.y > minRoomLength)
            {
                if (UnityEngine.Random.value <= 0.5f)
                {
                    if(currentRoom.size.y >= minRoomLength * 2)
                    {
                        YSplit(currentRoom, queueOfRooms, minRoomLength);
                    }
                    else if (currentRoom.size.x >= minRoomWidth * 2)
                    {
                        XSplit(currentRoom, queueOfRooms, minRoomWidth);
                    }
                    else if (currentRoom.size.x >= minRoomWidth && currentRoom.size.y >= minRoomLength)
                    {
                        listOfRooms.Add(currentRoom);
                    }
                }
                else
                {
                    if (currentRoom.size.x >= minRoomWidth * 2)
                    {
                        XSplit(currentRoom, queueOfRooms, minRoomWidth);
                    }
                    else if (currentRoom.size.y >= minRoomLength * 2)
                    {
                        YSplit(currentRoom, queueOfRooms, minRoomLength);
                    }
                    else if (currentRoom.size.x >= minRoomWidth && currentRoom.size.y >= minRoomLength)
                    {
                        listOfRooms.Add(currentRoom);
                    }
                }
            }

        }   
        return listOfRooms;
    }

    private static void YSplit(BoundsInt room, Queue<BoundsInt> queueOfRooms, int minRoomLength)
    {
        var splitRooms = UnityEngine.Random.Range(1, room.size.y);
        BoundsInt firstRoom, secondRoom;
        firstRoom = new BoundsInt(room.min, new Vector3Int(room.size.x, splitRooms, room.size.z));
        secondRoom = new BoundsInt(new Vector3Int(room.min.x, room.min.y + splitRooms, room.min.z),
            new Vector3Int(room.size.x, room.size.y - splitRooms, room.size.z));
        queueOfRooms.Enqueue(firstRoom);
        queueOfRooms.Enqueue(secondRoom);
    }

    private static void XSplit(BoundsInt room, Queue<BoundsInt> queueOfRooms, int minRoomWidth)
    {
        var splitRooms = UnityEngine.Random.Range(1, room.size.x);
        BoundsInt firstRoom, secondRoom;    
        firstRoom = new BoundsInt(room.min, new Vector3Int(splitRooms, room.size.y, room.size.z));
        secondRoom = new BoundsInt(new Vector3Int(room.min.x + splitRooms, room.min.y, room.min.z),
            new Vector3Int(room.size.x - splitRooms, room.size.y, room.size.z));
        queueOfRooms.Enqueue(firstRoom);
        queueOfRooms.Enqueue(secondRoom);
    }

    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int start, int walkLen)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(start);
        Vector2Int current = start;
        for (int i = 0; i < walkLen; i++)
        {
            Vector2Int next = current + Direction2D.getRandomDirection();
            path.Add(next);
            current = next;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int start, int corridorLen)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.getRandomDirection();
        var currentPos = start;
        corridor.Add(currentPos);
        for(int i = 0; i < corridorLen; i++)
        {
            currentPos += direction;
            corridor.Add(currentPos);
        }
        return corridor;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> directionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1), // SUS
        new Vector2Int(1, 0), // DREAPTA
        new Vector2Int(0, -1), // JOS
        new Vector2Int(-1, 0) // STANGA
    };

    public static Vector2Int getRandomDirection()
    {
        return directionsList[UnityEngine.Random.Range(0, directionsList.Count)];
    }
}

