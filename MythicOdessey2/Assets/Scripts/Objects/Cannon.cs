using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Trap,IInteracteable
{
    private bool _loaded;
    private Transform _charge;
    [SerializeField] private Transform _cannonModel;
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
        if (_loaded)
        {
            _cannonModel.LookAt(Helper.GetMouseWorldPosition());
            _cannonModel.eulerAngles = new Vector3(0, _cannonModel.localEulerAngles.y -75, 0);
        }

        if (Input.GetMouseButtonDown(0) && _loaded)
        {
            Shoot();
        }
    }

    public void DesInteraction()
    {
        throw new System.NotImplementedException();
    }

    void Reload()
    {
        _loaded = true;
        PlayerManager.Instance.EnterCannon();
        Debug.Log("Canon recargado");
    }

    void Shoot()
    {
        _loaded = false;
        PlayerManager.Instance.ExitCannon(Helper.GetMouseWorldPosition());
        Debug.Log("Disparando");
        EventManager.instance.TriggerEvent("OnTrapDestroyed",gridPosition);
        Destroy(gameObject);
    }
}
