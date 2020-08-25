using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoadCreator : MonoBehaviour
{
    public UnityEvent CreateRoad;
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CreateRoad.Invoke();
        }
    }
}
