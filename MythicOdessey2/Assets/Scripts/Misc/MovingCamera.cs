using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour {
    private Camera _cam;
    private float _movHor;
    private float _movVer;
    private Vector3 _dir;
   [SerializeField] private float _speed = 10;
    private void Awake(){
        _cam = Camera.main;
    }
    void Update(){
        _movHor = Input.GetAxis("Horizontal");
        _movVer = Input.GetAxis("Vertical");
        _dir.x = _movHor;
        _dir.z = _movVer;
        // _cam.orthographicSize -= _dir.z;
        _speed += Input.mouseScrollDelta.y;
        _cam.transform.Translate(_dir * Time.deltaTime * _speed);
        float newRotationX = _cam.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 2;
        float newRotationY = _cam.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * 2;
        _cam.transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
    }
}
