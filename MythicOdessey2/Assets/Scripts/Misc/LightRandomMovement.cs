using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VLB;
using Random = UnityEngine.Random;

public class LightRandomMovement : MonoBehaviour
{
    private float _currentTime;
    private float _startTime;
    private float _timer;
    private float _startTimeLight = .2f;
    private int _currColor;
    [SerializeField]private VolumetricLightBeam _light;
    [SerializeField] private int _minY;
    [SerializeField] private int _minZ;
    [SerializeField] private int _maxY;
    [SerializeField] private int _maxZ;
    [SerializeField] private int _x;
    private bool _10Sec;
    void Start()
    {
        _light = GetComponentInChildren<VolumetricLightBeam>();
        _timer = _startTimeLight;
        dadad();
    }

    private void Update()
    {
        if (_timer > 0 && _10Sec)
        {
            _timer -= Time.deltaTime * TimeManager.Instance.currentTimeScale;
            if (_timer <= 0)
            {
                switch (_currColor)
                {
                    case 0:
                        _light.color = Color.red;
                        _currColor = 1;
                        break;
                    case 1:
                        _light.color = Color.white;
                        _currColor = 0;
                        break;
                }
                _light.UpdateAfterManualPropertyChange();
                _timer = _startTimeLight;
            }
        }
    }

    private void dadad()
    {
        
        var y = Random.Range(_minY, _maxY);
        var z = Random.Range(_minZ, _maxZ);
        transform.DORotate(new Vector3(_x, y, z), 1.5f).OnComplete(dadad);
    }
    
    private void OnEnable()
    {
        EventManager.instance.AddAction("TenSecondsLeft",(x)=> _10Sec = true);
    }

    private void OnDisable()
    {
        EventManager.instance.RemoveAction("TenSecondsLeft",(x)=> _10Sec = true);
    }
}
