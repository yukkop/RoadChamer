using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkControler : MonoBehaviour
{
    public enum Sides
    {
        Down,
        Up,
        Left,
        Right
    }

    public enum WaysNames
    {
        Cross,
        DownDeadEnd,
        DownToLeft,
        LeftAndRidhtDeadEnd,
        LeftDeadEnd,
        LeftToRight,
        LeftToUp,
        RightDeadEnd,
        RightToDown,
        UpAndDownDeadEnd,
        UpDeadEnd,
        UpToDown,
        UpToRight,
        WithoutDown,
        WithoutLeft,
        WithoutRight,
        WithoutUp
    }

    public enum WayType
    {
        Road,
        Rail,
        Hybrid
    }

    public enum ChunkType
    {
        Water,
        Coast,
        Plane,
        Rock,
        River,
        City
    }

    public enum TilemapTypes
    {
        WaterLevel,
        GroundLevel,
        RoadLevel,
        RockLevel,
        Collider
    }

    //public Dictionary<AbstractChunk, Dictionary<AbstractChunk, bool>> roadGraph { get; private set; } = new Dictionary<AbstractChunk, Dictionary<AbstractChunk, bool>>();

    private List<Region> regions;
    private WayTiles wayTiles;
    private Chunk[,] map;
    private Vector2Int mapSize;
    private Dictionary<TilemapTypes, Tilemap> tilemaps = new Dictionary<TilemapTypes, Tilemap>();

    public void SetParametrs(WayTiles wayTiles, List<Region> regions, Vector2Int mapSize, float[,] heights)
    {
        this.wayTiles = wayTiles;
        this.regions = regions;
        this.mapSize = mapSize;
        {
            Tilemap[] tilemaps = this.transform.GetChild(0).GetComponentsInChildren<Tilemap>();
            this.tilemaps.Add(TilemapTypes.WaterLevel, Array.Find<Tilemap>(
                tilemaps, 
                new Predicate<Tilemap>(x => x.gameObject.name.Contains("Water")
                )));
            this.tilemaps.Add(TilemapTypes.GroundLevel, Array.Find<Tilemap>(
                tilemaps, 
                new Predicate<Tilemap>(x => x.gameObject.name.Contains("Ground")
                )));
            this.tilemaps.Add(TilemapTypes.RoadLevel, Array.Find<Tilemap>(
                tilemaps,
                new Predicate<Tilemap>(x => x.gameObject.name.Contains("Road")
                )));
            this.tilemaps.Add(TilemapTypes.RockLevel, Array.Find<Tilemap>(
                tilemaps, 
                new Predicate<Tilemap>(x => x.gameObject.name.Contains("Rock")
                )));
            this.tilemaps.Add(TilemapTypes.Collider, Array.Find<Tilemap>(
                tilemaps,
                new Predicate<Tilemap>(x => x.gameObject.name.Contains("Collider")
                )));
        }

        map = new Chunk[mapSize.x, mapSize.y];
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                map[x, y] = new Chunk();
                map[x, y].position = new Vector2Int(x, y);
            }
        }
        for (int y = 0; y < mapSize.y; y++) 
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                map[x, y].height = heights[x, y];
                if (x - 1 >= 0) { map[x, y].neigbours.Add(Sides.Left, map[x - 1, y]); map[x, y].wayAdjacent.Add(map[x - 1, y], Chunk.WayType.None); } // левый
                if (x + 1 < mapSize.x) { map[x, y].neigbours.Add(Sides.Right, map[x + 1, y]); map[x, y].wayAdjacent.Add(map[x + 1, y], Chunk.WayType.None); } // правый
                if (y - 1 >= 0) { map[x, y].neigbours.Add(Sides.Down, map[x, y - 1]); map[x, y].wayAdjacent.Add(map[x, y - 1], Chunk.WayType.None); } // нижней
                if (y + 1 < mapSize.y) { map[x, y].neigbours.Add(Sides.Up, map[x, y + 1]); map[x, y].wayAdjacent.Add(map[x, y + 1], Chunk.WayType.None); } // верхний
            }
        }
    }

    public void BuildChunk(Vector2Int position)
    {
        foreach (Region current in regions)
        {
            if (current.height > map[position.x, position.y].height)
            {
                map[position.x, position.y].type = current.name;

                ChoseDifficulty(position, current);

                break;
            }
        }
    }

    public void DisplayChunk(Vector2Int position)
    {
        foreach (Region current in regions)
        {
            if (current.height > map[position.x, position.y].height)
            {
                tilemaps[current.level].SetTile(new Vector3Int(position.x, position.y, 0), current.tileSet[0]);
                break;
            }
        }
    }

    void ChoseDifficulty(Vector2Int position, Region current) //Это надо перенести в поиск пути. (должен определять сложность по поху поиска)
    {
        float tempMaxHeight = (map[position.x, position.y].height < 1) ? map[position.x, position.y].height : map[position.x, position.y].height - 1;
        if (current.difficulty != 2)
        {
            map[position.x, position.y].difficulty = current.difficulty;
        }
        else
        {
            int regularLX = position.x, regularDY = position.y, regularRX = position.x, regularUY = position.y, distans = 0;
            while (true)
            {
                if (map[position.x, (--regularDY >= 0) ? regularDY : position.y].height > tempMaxHeight)
                {
                    map[position.x, position.y].difficulty = 2 + distans;
                    break;
                }
                else if (map[(--regularLX >= 0) ? regularLX : position.x, position.y].height > tempMaxHeight)
                {
                    map[position.x, position.y].difficulty = 2 + distans;
                    break;
                }
                else if (map[position.x, (++regularUY < mapSize.y) ? regularUY : position.y].height > tempMaxHeight)
                {
                    map[position.x, position.y].difficulty = 2 + distans;
                    break;
                }
                else if (map[(++regularRX < mapSize.x) ? regularRX : position.x, position.y].height > tempMaxHeight)
                {
                    map[position.x, position.y].difficulty = 2 + distans;
                    break;
                }
                distans++;
            }
        }
    }

    public Chunk[,] GetChunkMap()
    {
        return map;
    }

    public string GetChunkInfo(Vector2Int position)
    {
        return "";
    }

    public MapChunk GetChunk(Vector2Int position)
    {
        return new MapChunk();
    }

    public void UpdateRoad(Vector2Int position)
    {
        UpdateRoad(map[position.x, position.y]);
    }

    public void UpdateRoad(Chunk chunk)
    {
        if (chunk.wayType == Chunk.WayType.Road)
        {
            int choosenDirrection = -1;
            int choosenWayType = 0;
            int adjacentCount = 0;

            switch (chunk.type)
            {
                case (ChunkType.Plane):
                    choosenWayType = 0;
                    break;
                case (ChunkType.Water):

                    choosenWayType = 1;
                    break;
                case (ChunkType.River):

                    choosenWayType = 1;
                    break;
                case (ChunkType.City):
                    chunk.wayType =  Chunk.WayType.None;
                    return;
            }

            //try { roadGraph.Add(chunk, new Dictionary<AbstractChunk, bool>()); } catch { }

            Dictionary<Sides, bool> sides = new Dictionary<Sides, bool>();
            {
                sides.Add(Sides.Up, false);
                sides.Add(Sides.Down, false);
                sides.Add(Sides.Left, false);
                sides.Add(Sides.Right, false);
            }

            foreach (var currentNeigbour in chunk.neigbours)
            {
                foreach (var currentSide in sides)
                {
                    if (currentNeigbour.Key == currentSide.Key)
                    {
                        if (currentNeigbour.Value.wayType == Chunk.WayType.Road) {
                            sides[currentSide.Key] = true;
                            adjacentCount++;

                            chunk.wayAdjacent[currentNeigbour.Value] = Chunk.WayType.Road;

                            //try { roadGraph[chunk].Add(currentNeigbour.Value, true); } 
                            //catch { roadGraph[chunk][currentNeigbour.Value] = true; }
                            break;
                        }
                        chunk.wayAdjacent[currentNeigbour.Value] = Chunk.WayType.None;
                        //try { roadGraph[chunk].Add(currentNeigbour.Value, false); }
                        //catch { roadGraph[chunk][currentNeigbour.Value] = false; }
                    }
                }
                
            }

            switch (adjacentCount)
                {
                    case (0):
                        List<WaysNames> ways = new List<WaysNames>();
                        {
                            ways.Add(WaysNames.UpAndDownDeadEnd);
                            ways.Add(WaysNames.LeftAndRidhtDeadEnd);
                        }
                        choosenDirrection = (int)CustomLib.RundomEnum<WaysNames>(ways);
                        break;
                    case (1):
                        if (sides[Sides.Down])
                        {
                            choosenDirrection = (int)WaysNames.UpDeadEnd;
                        }
                        else if (sides[Sides.Up])
                        {
                            choosenDirrection = (int)WaysNames.DownDeadEnd;
                        }
                        else if (sides[Sides.Right])
                        {
                            choosenDirrection = (int)WaysNames.LeftDeadEnd;
                        }
                        else
                        {
                            choosenDirrection = (int)WaysNames.RightDeadEnd;
                        }
                        break;
                    case (2):
                        if (sides[Sides.Up])
                        {
                            if (sides[Sides.Down])
                            {
                                choosenDirrection = (int)WaysNames.UpToDown;
                            }
                            else if (sides[Sides.Left])
                            {
                                choosenDirrection = (int)WaysNames.LeftToUp;
                            }
                            else if (sides[Sides.Right])
                            {
                                choosenDirrection = (int)WaysNames.UpToRight;
                            }
                        }
                        else if (sides[Sides.Down])
                        {
                            if (sides[Sides.Left])
                            {
                                choosenDirrection = (int)WaysNames.DownToLeft;
                            }
                            else if (sides[Sides.Right])
                            {
                                choosenDirrection = (int)WaysNames.RightToDown;
                            }
                        }
                        else
                        {
                            choosenDirrection = (int)WaysNames.LeftToRight;
                        }
                        break;
                    case (3):
                        if (!sides[Sides.Up])
                        {
                            choosenDirrection = (int)WaysNames.WithoutUp;
                        }
                        else if (!sides[Sides.Down])
                        {
                            choosenDirrection = (int)WaysNames.WithoutDown;
                        }
                        else if (!sides[Sides.Right])
                        {
                            choosenDirrection = (int)WaysNames.WithoutRight;
                        }
                        else
                        {
                            choosenDirrection = (int)WaysNames.WithoutLeft;
                        }
                        break;
                    case (4):
                        choosenDirrection = (int)WaysNames.Cross;
                        break;
                }
            tilemaps[TilemapTypes.Collider].SetTile(new Vector3Int(chunk.position.x, chunk.position.y, 0), wayTiles.collider[choosenDirrection]);
            tilemaps[TilemapTypes.RoadLevel].SetTile(new Vector3Int(chunk.position.x, chunk.position.y, 0), wayTiles.typeOfWays[choosenWayType].road[choosenDirrection]);
        }
        else
        {
            //roadGraph.Remove(chunk);
            tilemaps[TilemapTypes.Collider].SetTile(new Vector3Int(chunk.position.x, chunk.position.y, 0), null);
            tilemaps[TilemapTypes.RoadLevel].SetTile(new Vector3Int(chunk.position.x, chunk.position.y, 0), null);
        }

        //Debug.Log("==========");
        //foreach(var currentDictionary in roadGraph)
        //{
        //    Debug.Log(currentDictionary.Key.position + ":");
        //    foreach (var current in roadGraph[currentDictionary.Key])
        //    {
        //        Debug.Log(current.Key.position + ":" + current.Value);
        //    }
        //}
    }

    public void UpdateRail(Vector2Int position)
    {

    }

    public void UpdateHybrid(Vector2Int position)
    {

    }

    public void AddRoad(Vector2Int position)
    {
        if (position.x < mapSize.x && position.y < mapSize.y && position.x >= 0 && position.y >= 0) 
        {
            switch (map[position.x, position.y].wayType)
            {
                case Chunk.WayType.None:
                    map[position.x, position.y].wayType = Chunk.WayType.Road;
                    UpdateRoad(position);
                    foreach (var current in map[position.x, position.y].neigbours)
                    {
                        if (current.Value.wayType == Chunk.WayType.Road)
                        {
                            UpdateRoad(current.Value.position);
                        }
                    }
                    break;
                case Chunk.WayType.Rail: //Неработает. Адейтить соседий надо в зависимости от их остояния
                    map[position.x, position.y].wayType = Chunk.WayType.Hybrid;
                    UpdateHybrid(position);
                    break;
            }
        }
    }

    public void RemoveRoad(Vector2Int position)
    {
        if (position.x < mapSize.x && position.y < mapSize.y && position.x >= 0 && position.y >= 0)
        {
            switch (map[position.x, position.y].wayType)
            {
                case Chunk.WayType.Road:
                    map[position.x, position.y].wayType = Chunk.WayType.None;
                    UpdateRoad(position);
                    foreach (var current in map[position.x, position.y].neigbours)
                    {
                        if (current.Value.wayType == Chunk.WayType.Road)
                        {
                            UpdateRoad(current.Value.position);
                        }
                    }
                    break;
            }
        }
    }

    public void AddRail(Vector2Int position)
    {
        switch (map[position.x, position.y].wayType)
        {
            case Chunk.WayType.None:
                map[position.x, position.y].wayType = Chunk.WayType.Rail;
                UpdateRail(position);
                foreach (var current in map[position.x, position.y].neigbours)
                {
                    UpdateRail(current.Value.position);
                }
                break;
            case Chunk.WayType.Road: //Неработает. Адейтить соседий надо в зависимости от их остояния
                map[position.x, position.y].wayType = Chunk.WayType.Hybrid;
                UpdateHybrid(position);
                break;
        }
    }
}
