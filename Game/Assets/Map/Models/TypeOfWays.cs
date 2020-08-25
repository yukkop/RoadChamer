using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class WayTiles
{
    public List<TypeOfWays> typeOfWays;
    public List<TileBase> collider;
}

[System.Serializable]
public class TypeOfWays
{
    public List<TileBase> road;
    public List<TileBase> rail;
    public List<TileBase> hybrid;
}
