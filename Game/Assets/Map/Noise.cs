using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    public static float[,] Generate(Vector2Int mapSize, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapSize.x, mapSize.y];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) scale = 0.001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapSize.x / 2f;
        float halfHeight = mapSize.y / 2f;

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int oct = 0; oct < octaves; oct++)
                {
                    float simpleX = (x - halfWidth) / scale * frequency + octaveOffsets[oct].x;
                    float simpleY = (y - halfHeight) / scale * frequency + octaveOffsets[oct].y;

                    float perlinValue = Mathf.PerlinNoise(simpleX, simpleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    internal static void Generate(object mapWidth, object mapHeight, object scale, object seed, object octaves, object persistance, object lacunarity, Vector2 vector2)
    {
        throw new NotImplementedException();
    }
}
