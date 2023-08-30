using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueEnemy : MonoBehaviour {
    private Rigidbody _rigidbody;
    [SerializeField] private float _torqueSpeed;
    private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private float _timeToStunt = 5;
    [SerializeField] private float _startTimer;
    private bool _isInvoekd;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _enemyStateMachine = GetComponent<EnemyStateMachine>();
    }

    private void Update(){
        if (_startTimer <= _timeToStunt)
        {
            _rigidbody.AddTorque(Vector3.one * _torqueSpeed);
            _startTimer += Time.deltaTime;
        }
        else
        {
            _rigidbody.isKinematic = true;
            if (!_isInvoekd)
            {
                Invoke("ResetTimer", 2f);
                _enemyStateMachine.StopChasing = true;
                _isInvoekd = true;
            }
        }
    }

    void ResetTimer(){
        _startTimer = 0;
        _enemyStateMachine.StopChasing = false;
        _isInvoekd = false;
        _rigidbody.isKinematic = false;
    }
}