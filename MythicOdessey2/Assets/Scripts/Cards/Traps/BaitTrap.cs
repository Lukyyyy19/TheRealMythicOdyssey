using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitTrap : Trap
{
    private void Awake()
    {
        Destroy(gameObject,10f);
    }

    private void OnDestroy()
    {
        EventManager.instance.TriggerEvent("OnTrapDestroyed",gridPosition);
    }
}
