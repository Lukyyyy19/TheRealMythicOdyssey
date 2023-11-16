using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerScene : MonoBehaviour
{
    private TextMeshPro _time;
    private float _startTime = .2f;
    private float _timer;
    private int _currColor = 0;
    private bool _10Sec;
    private void Awake()
    {
        _timer = _startTime;
        _time = GetComponent<TextMeshPro>();
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
                        _time.color = Color.red;
                        _currColor = 1;
                        break;
                    case 1:
                        _time.color = Color.white;
                        _currColor = 0;
                        break;
                }

                _timer = _startTime;
            }
        }
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
