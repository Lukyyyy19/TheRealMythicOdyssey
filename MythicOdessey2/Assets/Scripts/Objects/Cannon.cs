using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour,IInteracteable
{
    private bool _loaded;
    private Transform _charge;
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
            transform.LookAt(Helper.GetMouseWorldPosition());
            transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y -75, 0);
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
        Destroy(transform.parent.gameObject);
    }
}
