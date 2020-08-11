using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    float[,] heightsGraph;
    Vector2Int mapSize;

    public Map(Vector2Int mapSize, float[,] heightsGraph)
    {
        this.heightsGraph = heightsGraph;
        this.mapSize = mapSize;

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {

            }
        }

    }
}
