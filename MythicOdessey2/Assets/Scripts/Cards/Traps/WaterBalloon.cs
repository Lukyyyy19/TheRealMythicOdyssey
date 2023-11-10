using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterBalloon : Trap
{

    //[SerializeField] private GameObject _splashVFX;
    [SerializeField] private ParticleSystem _splashParticle1;
    [SerializeField] private ParticleSystem _splashParticle2;

    private void Awake()
    {
        transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        transform.DOMoveY(0.5f, .35f).SetEase(Ease.InCirc).OnComplete((() =>
        {
            var cols = Physics.OverlapSphere(transform.position, 3f);
            
            
            
            foreach (var col in cols)
            {
                if (col.TryGetComponent(out IDamageable _damageable))
                {
                    
                    _damageable.TakeDamage(1, transform);
                }
            }
            ShowParticles();
            Destroy(gameObject,.4f);
        }));
    }

    private void OnDestroy()
    {
        EventManager.instance.TriggerEvent("OnTrapDestroyed", gridPosition);
    }
    
    private void ShowParticles()
    {
        //Instantiate(_splashVFX, transform.position, Quaternion.identity);
        _splashParticle1.Play();
        _splashParticle2.Play();
    }
}
