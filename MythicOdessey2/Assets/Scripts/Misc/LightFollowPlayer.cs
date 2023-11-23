using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _lightCube;
    void Update()
    {
        transform.LookAt(_lightCube);
    }
}
