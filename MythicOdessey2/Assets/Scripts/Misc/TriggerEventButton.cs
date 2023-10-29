using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventButton : MonoBehaviour
{
    public void TriggerEvent(string name)
    {
        EventManager.instance.TriggerEvent(name);
    }
}
