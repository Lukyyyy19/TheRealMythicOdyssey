using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LightRandomMovement : MonoBehaviour
{
    private float _currentTime;
    private float _startTime;
    void Start()
    {
        dadad();
    }

    private void dadad()
    {
        var x = Random.Range(0, 90);
        var y = Random.Range(100, 360);
        var z = 40;
        transform.DORotate(new Vector3(x, y, z), 1.5f).OnComplete(dadad);
    }
}
