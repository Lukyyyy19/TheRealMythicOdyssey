using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamageable {
    private bool _damageTaken;
    private float _health = 2;
    public void TakeDamage(int damage,Transform attacker){
        if(_damageTaken)return;
        _damageTaken = true;
        Debug.Log("Enemy took damage");
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Die(){
        Destroy(gameObject);
    }

    private void OnEnable(){
        EventManager.instance.AddAction("OnPlayerAttackFinished", (object[] args) => {
            _damageTaken = false;
        });
    }
}
