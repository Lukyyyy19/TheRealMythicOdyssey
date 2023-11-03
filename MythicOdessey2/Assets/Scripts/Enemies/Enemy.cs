using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    private bool _damageTaken;
    private float _health = 2;

    public void TakeDamage(int damage, Transform attacker)
    {
        if (_damageTaken) return;
        _damageTaken = true;
        
       
        
    }

    private void OnEnable()
    {
        EventManager.instance.AddAction("OnPlayerAttackFinished", (object[] args) => { _damageTaken = false; });
    }
}
