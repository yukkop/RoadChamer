using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk 
{
    public enum WayType {None, Road, Rail, Hybrid }
    public Vector2Int position;
    public float height;

    public ChunkControler.ChunkType type;
    public WayType wayType = WayType.None;
    public int difficulty;
    public Dictionary<ChunkControler.Sides, Chunk> neigbours = new Dictionary<ChunkControler.Sides, Chunk>();
    public Dictionary<Chunk, WayType> wayAdjacent = new Dictionary<Chunk, WayType>(); 

    public int Heurist(Chunk target)
    {
        return Mathf.Abs(this.position.x - target.position.x) + Mathf.Abs(this.position.y - target.position.y);
    }
}
