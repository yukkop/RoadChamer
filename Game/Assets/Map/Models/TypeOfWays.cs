using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TypeOfWays
{
    public TileBase road;
    public TileBase rail;
    public TileBase hybrid;

    public TypeOfWays(TileBase road, TileBase rail, TileBase hybrid)
    {
        this.road = road;
        this.rail = rail;
        this.hybrid = hybrid;
    }
}
