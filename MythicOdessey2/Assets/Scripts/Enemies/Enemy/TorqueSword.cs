using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueSword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if (TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(1);
        }
    }
}
