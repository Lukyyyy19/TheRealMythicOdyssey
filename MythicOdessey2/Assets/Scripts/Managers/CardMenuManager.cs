using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardMenuManager : MonoBehaviour
{
    static CardMenuManager _instance;
    public bool menuOpen;
    public GameObject cardMenu;
    public static CardMenuManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void OpenMenu(bool open)
    {
        TimeManager.Instance.currentTimeScale = open ? 0.25f : 1;
        EventManager.instance.TriggerEvent("OnTimeChanged",TimeManager.Instance.currentTimeScale);
        EventManager.instance.TriggerEvent("OnOpenMenu",open);
        cardMenu.SetActive(open);
    }
}
