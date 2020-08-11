using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCell : MonoBehaviour
{
    public string type;
    public bool Road, Rail;
    public List<AbstractCell> neigbours;
}
