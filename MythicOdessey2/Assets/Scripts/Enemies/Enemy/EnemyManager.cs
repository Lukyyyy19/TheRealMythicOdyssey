using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] protected EnemyFactoryState _states;
    private EnemyStateMachine _enemyStateMachine;
    public EnemyFactoryState States => _states;

    private void Awake(){
        _enemyStateMachine = GetComponent<EnemyStateMachine>();
        _states = new EnemyFactoryState(_enemyStateMachine);
    }
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.TryGetComponent(out IDamageable damageable) && damageable is PlayerManager)
        {
            damageable.TakeDamage(1,transform);
        }
    }
}