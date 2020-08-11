using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public struct Region 
{
    public string name;
    public float height;
    public List<TileBase> tileSet;
}
