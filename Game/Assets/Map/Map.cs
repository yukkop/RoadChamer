using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Vector2Int mapSize;
    public ChunkControler chunkControler;



    public void SetParameters(Vector2Int mapSize, float[,] heightsGraph, WayTiles wayTiles, List<Region> regions)
    {
        this.mapSize = mapSize;
        chunkControler = this.gameObject.AddComponent(typeof(ChunkControler)) as ChunkControler;
        chunkControler.SetParametrs(wayTiles, regions, mapSize, heightsGraph);
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                chunkControler.BuildChunk(new Vector2Int(x, y));
            }
        }
    }

    //public Dictionary<AbstractChunk, Dictionary<AbstractChunk, bool>> GetRoadGraph()
    //{
    //    return chunkControler.roadGraph;
    //}

    public void DisplayMap()
    {
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                chunkControler.DisplayChunk(new Vector2Int(x, y));
            }
        }
    }

    public void CreateRoad(Vector2Int position)
    {
        chunkControler.AddRoad(position);
    }

    public void DeleteRoad(Vector2Int position)
    {
        chunkControler.RemoveRoad(position);
    }

    private void GenerateNewMap()
    {
        MapGenerator generator = this.gameObject.AddComponent(typeof(MapGenerator)) as MapGenerator;

        Destroy(this);
    }

    public Chunk[,] GetChunkMap()
    {
        return chunkControler.GetChunkMap();
    }
}
