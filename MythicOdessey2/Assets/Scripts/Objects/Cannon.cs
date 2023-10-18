using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Trap, IInteracteable
{
    private bool _loaded;
    private Transform _charge;
    [SerializeField] private Transform _cannonModel;

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
        if (!_loaded) return;

        _timer += Time.deltaTime;
        _cannonModel.LookAt(Helper.GetMouseWorldPosition());
        _cannonModel.eulerAngles = new Vector3(0, _cannonModel.localEulerAngles.y - 75, 0);

        if (Input.GetMouseButtonDown(0) || _timer >= _maxTimer)
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
        PlayerManager.Instance.EnterCannon();
        Debug.Log("Canon recargado");
    }

    private void Shoot()
    {
        _loaded = false;
        PlayerManager.Instance.ExitCannon(Helper.GetMouseWorldPosition());
        Debug.Log("Disparando");
        EventManager.instance.TriggerEvent("OnTrapDestroyed", gridPosition);
        Destroy(gameObject);
    }
}
