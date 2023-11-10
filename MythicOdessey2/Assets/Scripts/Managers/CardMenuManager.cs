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
    public bool menuClose;
    
    public static CardMenuManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void OpenMenu(bool open)
    {
        TimeManager.Instance.currentTimeScale = open ? 0.25f : 1;
        EventManager.instance.TriggerEvent("OnTimeChanged", TimeManager.Instance.currentTimeScale);
        EventManager.instance.TriggerEvent("OnOpenMenu", open);
        cardMenu.SetActive(open);
        
        /*
        if (PlayerManager.Instance._magic <= 0)
        {
            cardMenu.SetActive(false);
            Debug.Log("No puedes abrir por no tener mana");
        }
        else
        {
            cardMenu.SetActive(open);
            Debug.Log("Abriendo menu");
        }
        */
    }
    
    public void CloseMenu(bool close)
    {
        cardMenu.SetActive(close);
    }
}
