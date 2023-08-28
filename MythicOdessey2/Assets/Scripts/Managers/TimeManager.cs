using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    static TimeManager _instance;
    public static TimeManager Instance => _instance;
    public float currentTimeScale;
    private int tick;
    private float tickTimer;
    private const float TICK_TIMER_MAX = 0.5f;
    private void Awake(){
        _instance = this;
        tick = 0;
    }

    private void Update(){
        tickTimer += Time.deltaTime;
        if(tickTimer>= TICK_TIMER_MAX){
            tickTimer -= TICK_TIMER_MAX;
            tick++;
        }
    }
}
