using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLib : MonoBehaviour
{
    public static T RundomEnum<T>(T[] values)
    {
        return values[new System.Random().Next(0, values.Length)];
    }

    public static T RundomEnum<T>(List<T> values)
    {
        return values[new System.Random().Next(0, values.Count)];
    }

    public static LinkElement<Chunk> RoadFindPath(Chunk start, Chunk tartget)
    {
        List<Chunk> openedPoint = new List<Chunk>();
        List<Chunk> closedPoints = new List<Chunk>();

        openedPoint.Add(start);
        CheckNewPoint(tartget, openedPoint, closedPoints);
        return new LinkElement<Chunk>();
    }

    private static void CheckNewPoint(Chunk tartget, List<Chunk> openedPoint, List<Chunk> closedPoints)
    {
        Chunk current = ChooseNextPoint(openedPoint, tartget);

        if (current == tartget)
            return;

        openedPoint.Remove(current);
        closedPoints.Add(current);

        AddNewAdjecent(openedPoint, current, closedPoints);

        CheckNewPoint(tartget, openedPoint, closedPoints);
    }

    private static Chunk ChooseNextPoint(List<Chunk> openedPoints, Chunk target)
    {
        int min = int.MaxValue;
        Chunk newCurrent = new Chunk();

        foreach (Chunk current in openedPoints)
        {
            int temp = current.Heurist(target);

            if (min > temp)
            {
                min = temp;
                newCurrent = current;
            }
        }

        return newCurrent;
    }

    private static void AddNewAdjecent(List<Chunk> openedPoint, Chunk chunk, List<Chunk> closedPoints)
    {
        foreach (var currentAdjacent in chunk.wayAdjacent)
        {
            if (currentAdjacent.Value == Chunk.WayType.Road)
            {
                if (!IsChunkClosed(closedPoints, currentAdjacent.Key))
                {
                    if (!IsChunkOpened(closedPoints, currentAdjacent.Key))

                    {
                        openedPoint.Add(currentAdjacent.Key);
                    }
                }
            }
        }
    }

    private static bool IsChunkOpened(List<Chunk> openedPoint, Chunk chunk) 
    {
        foreach (var current in openedPoint)
        {
            if (chunk == current)
            {
                return true;
            }
        }
        return false;
    }
    private static bool IsChunkClosed(List<Chunk> closedPoints, Chunk chunk)
    {
        foreach (var current in closedPoints)
        {
            if (chunk == current)
            {
                return true;
            }
        }
        return false;
    }
}