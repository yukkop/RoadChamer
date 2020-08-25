using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;

public class Controler : MonoBehaviour
{
    public enum ActionsNames
    {
        RoadCreator,
        RoadDestroer,
        Movement
    }

    public List<MonoBehaviour> actions;
    public List<ClickButtonEvent> buttons;
    public Tilemap tilemap;

    private ActionsNames nowEnable;
    private Map map;

    void Start()
    {
        nowEnable = ActionsNames.Movement;
    }

    public void ControlActions(ActionsNames action)
    {
        if (nowEnable != ActionsNames.Movement)
            buttons[(int)nowEnable].PressOut();

        actions[(int)nowEnable].enabled = false;
        if (actions[(int)action] == actions[(int)nowEnable])
            nowEnable = ActionsNames.Movement;
        else
            nowEnable = action;
        actions[(int)nowEnable].enabled = true;
    }


    public void FindMap()
    {
        map = FindObjectOfType<Map>();
    }

    public void CreateRoad()
    {
        Vector3Int cellIndex = tilemap.WorldToCell(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0));
        Vector2Int position = new Vector2Int(cellIndex.x, cellIndex.y);
        map.CreateRoad(position);

        //Debug.Log(tilemap.CellToWorld(cellIndex)); //Лотдает корды левого нижнего угла ячейки 
    }

    public void DestroyRoad()
    {
        Vector3Int cellIndex = tilemap.WorldToCell(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0));
        Vector2Int position = new Vector2Int(cellIndex.x, cellIndex.y);
        map.DeleteRoad(position);
    }

    public void MoveTo()
    {
        Vector3Int cellIndex = tilemap.WorldToCell(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0));
        Vector2Int position = new Vector2Int(cellIndex.x, cellIndex.y);
    }

    void Update()
    {

    }
}
