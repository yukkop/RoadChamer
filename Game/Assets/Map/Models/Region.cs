using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public struct Region 
{
    public ChunkControler.ChunkType name;
    public ChunkControler.TilemapTypes level;
    public float height;
    public int difficulty;
    public List<TileBase> tileSet;
}