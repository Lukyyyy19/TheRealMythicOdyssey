using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingTiles : MonoBehaviour
{
    void Start()
    {
        TestGrid.instance.startTiles.Add(transform);
    }
}
