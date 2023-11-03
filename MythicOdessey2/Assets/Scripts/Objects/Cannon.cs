using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class Cannon : Trap, IInteracteable
{
    private bool _loaded;
    private Transform _charge;
    [SerializeField] private Transform _cannonModel;
    [SerializeField] private VisualEffect _smokeParticles;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private ParticleSystem _explosionParticles1;
    [SerializeField] private ParticleSystem _explosionParticles2;
    [SerializeField] private ParticleSystem _explosionParticles3;
    
    private float _timer;
    private float _maxTimer = 5f;

    public void Interaction()
    {
        if (!_loaded)
        {
            Reload();
        }
        else
        {
            Shoot();
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime * TimeManager.Instance.currentTimeScale;
        if (_timer >= _maxTimer)
        {
            DestroyCannon();
        }

        if (!_loaded) return;
        _cannonModel.LookAt(Helper.GetMouseWorldPosition());
        _cannonModel.eulerAngles = new Vector3(0, _cannonModel.localEulerAngles.y - 75, 0);
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void DesInteraction()
    {
        throw new System.NotImplementedException();
    }

    private void Reload()
    {
        _loaded = true;
        PlayerManager.Instance.EnterCannon(transform.position);
        Debug.Log("Canon recargado");
    }

    private void Shoot()
    {
        _loaded = false;
        PlayerManager.Instance.ExitCannon(Helper.GetMouseWorldPosition());
        _explosionParticles.Play();
        _explosionParticles1.Play();
        _explosionParticles2.Play();
        _explosionParticles3.Play();
        DestroyCannon();
    }

    void DestroyCannon()
    {
        EventManager.instance.TriggerEvent("OnTrapDestroyed", gridPosition);
        //Destroy(Instantiate(_smokeParticles, transform.position + new Vector3(2, 0, 2), quaternion.identity), 2f);
        Destroy(gameObject, 2f);
        if (_loaded)
        {
            PlayerManager.Instance.ExitCannon(transform.position);
        }
    }
}
