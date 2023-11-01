
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterBalloon : Trap
{
    private void Awake()
    {
        //var newpos = worldPosition + new Vector3(2,0,2);
        //transform.position = new Vector3(worldPosition.x,10,worldPosition.z);
        transform.position = new Vector3(transform.position.x, 10, transform.position.z);   
        transform.DOMoveY(0.5f, .35f).SetEase(Ease.InCirc).OnComplete((() =>
        {
            var cols = Physics.OverlapSphere(transform.position, 3f);
            foreach (var col in cols)
            {
                if (col.TryGetComponent(out IDamageable _damageable))
                {
                    _damageable.TakeDamage(1,transform);
                }
            }
        }));
    }
}
