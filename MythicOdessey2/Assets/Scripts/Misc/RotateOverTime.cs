using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 8 * Time.deltaTime*TimeManager.Instance.currentTimeScale);
    }
}
