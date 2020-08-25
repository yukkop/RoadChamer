 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class MapGenerator : MonoBehaviour
{
    [Header("Настройки Шума")]
    public Vector2Int mapSize;
    public float scale;
    public int seed;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public Vector2 offset;
    [Header("Реки")]
    public float riverScaleCoefficient;
    [Range(0, 1)]
    public float riverLowerValue;
    [Range(0, 1)]
    public float riverUpperValue;

    [Header("Карта")]
    public List<Region> regions;
    public WayTiles wayTiles;
    public int cityCount;

    public UnityEvent MapWasCreated;

    private List<Chunk> tempAvailableChunks;
    private Chunk tempCityCenter;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        if (seed == 0)
        {
            seed = System.DateTime.Now.Millisecond;
        }

        float[,] noisMap = Noise.Generate(mapSize, scale, seed, octaves, persistance, lacunarity, offset);
        float[,] mapHeights = noisMap;
        noisMap = Noise.Generate(mapSize, scale / riverScaleCoefficient, seed / 2, octaves, persistance, lacunarity, offset);

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                if (noisMap[x, y] > riverLowerValue && noisMap[x,y] < riverUpperValue)
                {
                    mapHeights[x, y] = 1 + noisMap[x, y];
                }
            }
        }

        Map map = this.gameObject.AddComponent(typeof(Map)) as Map;
        map.SetParameters(mapSize, mapHeights, wayTiles,  regions);

        GenerateCitys(map);

        map.DisplayMap();

        MapWasCreated.Invoke();

        Destroy(this);
    }

    public void GenerateCitys(Map map)
    {
        Chunk[,] mapChunks = map.GetChunkMap();
        tempAvailableChunks = new List<Chunk>();

        foreach (Chunk current in mapChunks)
        {
            if (current.type == ChunkControler.ChunkType.Plane)
            {
                tempAvailableChunks.Add(current);
            }
        }
        
        for (int i = 0; i < cityCount; i++)
        {
            tempCityCenter = CustomLib.RundomEnum<Chunk>(tempAvailableChunks);
            tempAvailableChunks.Remove(tempCityCenter);
            tempCityCenter.height = 10;
            foreach (var current in tempCityCenter.neigbours)
            {
                if (tempAvailableChunks.Find(element => element == current.Value) != null)
                {
                    //Debug.Log(tempAvailableChunks.Find(element => element == current).type);
                    NewCityChunk(current.Value);
                }
                //Debug.Log(tempCityCenter.neigbours);
            }
            List<Chunk> toRemove = new List<Chunk>();
            foreach (Chunk current in tempAvailableChunks) 
            {
                if (current.Heurist(tempCityCenter) < (mapSize.x + mapSize.y)/2)
                {
                    toRemove.Add(current);
                }
            }
            foreach(Chunk current in toRemove)
            {
                tempAvailableChunks.Remove(current);
            }
        }
    }

    private void NewCityChunk(Chunk cityChunk)
    {
        
        //Debug.Log(cityChunk.Heurist(tempCityCenter));
        if (cityChunk.Heurist(tempCityCenter) < 5) 
        {
            if (Random.Range(0, 5) != 0)
            {
                tempAvailableChunks.Remove(cityChunk);
                cityChunk.height = 10;
                cityChunk.type = ChunkControler.ChunkType.City;
                foreach (var current in cityChunk.neigbours)
                {
                    if (tempAvailableChunks.Find(element => element == current.Value) != null)
                    {
                        //Debug.Log(tempAvailableChunks.Find(element => element == current).type);
                        NewCityChunk(current.Value);
                    }
                }
            }
        }
    }
}