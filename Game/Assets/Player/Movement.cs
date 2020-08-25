using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    private Vector3 firstEntry, firsCamPos;
    private bool moveOn; 
    void Start()
    {

    }

    void Update()
    {
        if (moveOn == false) { moveOn = true; firstEntry = Input.mousePosition; firsCamPos = this.transform.position; }

        this.transform.position = firsCamPos - transform.TransformDirection(new Vector3(Input.mousePosition.x - firstEntry.x, Input.mousePosition.y - firstEntry.y, 0) * speed);
    }
}
