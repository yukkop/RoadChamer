using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Vector2Int mapSize;
    public float scale;
    public int seed;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public Vector2 offset;

    public List<Region> regions;

    List<List<TypeOfWays>> ways;
    public int CellTypeCount;
    public int WaysTypeCount;

    void Start()
    {
        for (int i = 0; i < CellTypeCount; i++)
        {
            ways.Add(new List<TypeOfWays>());
            for (int j = 0; j < WaysTypeCount; j++)
            {
                ways[i].Add(new TypeOfWays(Resources.Load($"{j}") as TileBase, Resources.Load($"{j}") as TileBase, Resources.Load($"{j}") as TileBase));
            }
        }

    }

    public void GenerateMap()
    {

        float[,] NoisMap = Noise.Generate(mapSize, scale, seed, octaves, persistance, lacunarity, offset);
        //Map map = new Map(mapSize, NoisMap, ways[0]);

    }
}
