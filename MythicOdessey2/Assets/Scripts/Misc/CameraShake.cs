using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;

    [SerializeField,Range(1,100)]private float _shakeIntensity = 86f;
    [SerializeField,Range(0,1f)]private float _shakeTimer = 1f;
    private float _shakeTimerTotal;
    CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    bool _isShaking = false;
    // Start is called before the first frame update
    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start(){
        StopShake();
    }

    void StopShake(){
        _isShaking = false;
        virtualCameraNoise.m_AmplitudeGain = 0;
        _shakeTimerTotal = _shakeTimer;
    }

    public void ShakeCamera(){
        virtualCameraNoise.m_AmplitudeGain = _shakeIntensity;
        _shakeTimerTotal = _shakeTimer;
        _isShaking = true;
    }

    private void Update(){
        if(_shakeTimerTotal > 0 && _isShaking){
            _shakeTimerTotal -= Time.deltaTime;
            if(_shakeTimerTotal <= 0){
                StopShake();
            }
        }
    }

    private void OnEnable(){
        EventManager.instance.AddAction("PlayerDamaged",objects => {
            Debug.Log("Shaking");
            ShakeCamera();
        });
    }
}