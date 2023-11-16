using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] protected EnemyFactoryState _states;
    private EnemyStateMachine _enemyStateMachine;
    public EnemyFactoryState States => _states;
    
    private bool _isTakingDamage;
    private float _timer;
    private float _interval = 1f;

    private void Awake(){
        _enemyStateMachine = GetComponent<EnemyStateMachine>();
        _states = new EnemyFactoryState(_enemyStateMachine);
    }

    private void Update()
    {
        if (_isTakingDamage)
        {
            _timer += Time.deltaTime;
            if (_timer >= _interval)
            {
                _timer = 0;
                _isTakingDamage = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            if (!_isTakingDamage)
            {
                if (_enemyStateMachine.isEnchanted && damageable is EnemyStateMachine)
                {
                    damageable.TakeDamage(1, transform);
                    _isTakingDamage = true;
                }
                else if(!_enemyStateMachine.isEnchanted && damageable is PlayerManager)
                {
                    damageable.TakeDamage(1, transform);
                    _isTakingDamage = true;
                }
            }
    }
}