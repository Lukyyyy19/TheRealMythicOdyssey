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
    [SerializeField]private VisualEffect _smokeParticles;
    private float _timer;
    private float _maxTimer = 3f;

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
        _timer += Time.deltaTime*TimeManager.Instance.currentTimeScale;
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
        DestroyCannon();
    }

    void DestroyCannon()
    {
        EventManager.instance.TriggerEvent("OnTrapDestroyed", gridPosition);
        Destroy(Instantiate(_smokeParticles, transform.position + new Vector3(2,0,2), quaternion.identity),2f);
        Destroy(gameObject);
        if (_loaded)
        {
            PlayerManager.Instance.ExitCannon(transform.position);
        }
    }
}
