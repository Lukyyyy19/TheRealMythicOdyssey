using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public class CardMenuManager : MonoBehaviour
{
    static CardMenuManager _instance;
    public bool menuOpen;
    public GameObject cardMenu;
    public bool menuClose;
    [SerializeField] private List<Card> _cardList;
    public static CardMenuManager Instance => _instance;
    [SerializeField]private int _currentCardSelected;
    private bool _isOrdered;
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
       OpenMenu(true);
        
    }

    private void Update()
    {
        ChangeSelected();
    }

    public void OpenMenu(bool open)
    {
        // TimeManager.Instance.currentTimeScale = open ? 0.25f : 1;
        // EventManager.instance.TriggerEvent("OnTimeChanged",TimeManager.Instance.currentTimeScale);
        // EventManager.instance.TriggerEvent("OnOpenMenu",open);
        cardMenu.SetActive(open);

        if (!_isOrdered)
        {
            _cardList = _cardList.OrderBy(x => x.Id).ToList();
            _isOrdered = true;
        }
        //TimeManager.Instance.currentTimeScale = open ? 0.25f : 1;
        // EventManager.instance.TriggerEvent("OnTimeChanged", TimeManager.Instance.currentTimeScale);
        // EventManager.instance.TriggerEvent("OnOpenMenu", open);
        //cardMenu.SetActive(open);
        menuOpen = open;
        if (menuOpen && _currentCardSelected == 0)
        {
            _currentCardSelected = 5;
            InteractCard();
        }
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

    public void AddCard(Card card)
    {
        _cardList.Add(card);
    }
    
    public void CloseMenu(bool close)
    {
        cardMenu.SetActive(close);
    }

    public void ChangeSelected()
    {
        //if(!menuOpen)return;
        // if (_currentCardSelected>=0&&_currentCardSelected<_cardList.Count)
        // {
        //     if (_currentCardSelected == _cardList.Count - 1)
        //     {
        //         _currentCardSelected = 0;
        //     }
        //     else
        //     {
        //     _currentCardSelected++;
        //     }
        // }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentCardSelectedInteraction(0,true);
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentCardSelectedInteraction(1,true);
        }else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentCardSelectedInteraction(2,true);
        }else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CurrentCardSelectedInteraction(3,true);
        }else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CurrentCardSelectedInteraction(4,true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
           CurrentCardSelectedInteraction(5,false);
        }
    }

    private void CurrentCardSelectedInteraction(int position,bool hands)
    {
        if (_currentCardSelected == position) return;
        PlayerManager.Instance.HasHandsOccupied = hands;
        _currentCardSelected = position;
        InteractCard();
    }

    private void InteractCard()
    {
        
        var cardTemp = _cardList[_currentCardSelected];
        cardTemp.Interaction();
        //     CardMenuManager.Instance.GetCurrentCardSelected().TriggerInstantiateEvent();
        cardTemp.TriggerInstantiateEvent();
        foreach (var card in _cardList)
        {
            if (card != cardTemp)
            {
                card.DesInteraction();
            }
        }
    }

    public Card GetCurrentCardSelected()
    {
        return _cardList[_currentCardSelected];
    }

    private void OnEnable()
    {
        EventManager.instance.AddAction("OnCardBuilt",(x)=>CurrentCardSelectedInteraction(5,false));
    }
}
