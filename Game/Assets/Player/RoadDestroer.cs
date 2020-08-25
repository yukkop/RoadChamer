using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoadDestroer : MonoBehaviour
{
    public UnityEvent DestroyRoad;
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DestroyRoad.Invoke();
        }
    }
}
