using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public Dictionary<string, Action<object[]>> eventDictionary = new Dictionary<string, Action<object[]>>();

    private void Awake()
    {
        instance = this;
    }

    public void AddAction(string actionName, Action<object[]> action)
    {
        if (eventDictionary.ContainsKey(actionName))
        {
            eventDictionary[actionName] += action;
            //Debug.Log($"añadiendo metodo a la key {actionName}");
        } else
        {
            eventDictionary.Add(actionName, action);
           // print($"Creando key {actionName} y añadiendo");
        }
    }


    public void RemoveAction(string actionName, Action<object[]> action)
    {
        // eventDictionary[actionName] -= action;
        if (eventDictionary.ContainsKey(actionName))
        {
            eventDictionary[actionName] -= action;
            //Debug.Log($"Removiendo metodo a la key {actionName}");
        } else
        {
            //eventDictionary.Add(actionName, action);
            //print($"No existe la key {actionName} para remover");
        }
    }


    public void TriggerEvent(string actionName, params object[] args)
    {
        if (eventDictionary.ContainsKey(actionName))
        {
            eventDictionary[actionName]?.Invoke(args);
            //Debug.Log($"LLamando al evento {actionName}");
        }
           // Debug.Log($"No contiene la key {actionName}");
    }

}
