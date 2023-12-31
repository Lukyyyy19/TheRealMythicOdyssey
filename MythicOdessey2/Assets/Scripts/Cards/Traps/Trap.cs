using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Trap : MonoBehaviour
{
    public List<Vector2Int> gridPosition;
    public Vector3 worldPosition;
    public Trap realTrap;
    public Transform woodBox;

    private void OnDisable()
    {
        if(woodBox!=null)
        Destroy(woodBox.gameObject);
    }
}
