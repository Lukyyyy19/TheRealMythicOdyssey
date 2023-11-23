using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightCube : MonoBehaviour
{
    private float _maxX = 14f;
    private float _minX = -14f;

    private float _startTime = 6f;

    private float _timer;
    private bool _startMove;
    private void Awake()
    {
        _timer = _startTime;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime*TimeManager.Instance.currentTimeScale;
            _startMove = true;
        }
        else
        {
            if (_startMove)
            {
                float x = Random.Range(_minX, _maxX);
                float z = Random.Range(_minX, _maxX);
                var newPos = new Vector3(x, 0, z);
                transform.DOMove(newPos, 3f).OnComplete((() => _timer = _startTime));
                _startMove = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerManager player))
        {
            player.ChargeMana(.3f*Time.deltaTime*TimeManager.Instance.currentTimeScale);
        }
    }
}
