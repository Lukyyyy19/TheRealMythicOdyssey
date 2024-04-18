using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCard : MonoBehaviour
{
    private Camera _cam;

    private void Awake()
    {
        var x = transform.rotation;
        // transform.rotation.eulerAngles = Vector3.up*180;
        transform.rotation = Quaternion.Euler(Vector3.up * 180);
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");
        // transform.Rotate(Vector3.right, y);
        // transform.Rotate(Vector3.down, x);

        var right = Vector3.Cross(_cam.transform.up, transform.position - _cam.transform.position);
        var up = Vector3.Cross(transform.position - _cam.transform.position, right);
        transform.rotation = Quaternion.AngleAxis(-x, up) * transform.rotation;
        var euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(ClampMagnitude01(euler,250,110));
        //transform.rotation = Quaternion.AngleAxis(y, right) * transform.rotation;
    }
    
    public Vector3 ClampMagnitude01(Vector3 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if(sm > (double)max * (double)max) return v.normalized * max;
        else if(sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }
}
