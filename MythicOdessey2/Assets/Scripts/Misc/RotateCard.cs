using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCard : MonoBehaviour
{
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void OnMouseDrag()
    {
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");
        // transform.Rotate(Vector3.right, y);
        // transform.Rotate(Vector3.down, x);

        var right = Vector3.Cross(_cam.transform.up, transform.position - _cam.transform.position);
        var up = Vector3.Cross(transform.position - _cam.transform.position, right);
        transform.rotation = Quaternion.AngleAxis(-x, up) * transform.rotation;
        //transform.rotation = Quaternion.AngleAxis(y, right) * transform.rotation;
    }
}
