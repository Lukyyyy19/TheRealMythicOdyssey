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
<<<<<<< Updated upstream
=======
    public bool menuClose;
    [SerializeField] private List<Card> _cardList;
>>>>>>> Stashed changes
    public static CardMenuManager Instance => _instance;
    [SerializeField]private int _currentCardSelected;
    private bool _isOrdered;
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        cardMenu.SetActive(false);
        
    }

    public void OpenMenu(bool open)
    {
<<<<<<< Updated upstream
        TimeManager.Instance.currentTimeScale = open ? 0.25f : 1;
        EventManager.instance.TriggerEvent("OnTimeChanged",TimeManager.Instance.currentTimeScale);
        EventManager.instance.TriggerEvent("OnOpenMenu",open);
        cardMenu.SetActive(open);
=======
        if (!_isOrdered)
        {
            _cardList = _cardList.OrderBy(x => x.Id).ToList();
            _isOrdered = true;
        }
        //TimeManager.Instance.currentTimeScale = open ? 0.25f : 1;
        EventManager.instance.TriggerEvent("OnTimeChanged", TimeManager.Instance.currentTimeScale);
        EventManager.instance.TriggerEvent("OnOpenMenu", open);
        cardMenu.SetActive(open);
        menuOpen = open;
        if (menuOpen && _currentCardSelected == 0)
        {
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
>>>>>>> Stashed changes
    }

    public void ChangeSelected()
    {
        if(!menuOpen)return;
        if (_currentCardSelected>=0&&_currentCardSelected<_cardList.Count)
        {
            if (_currentCardSelected == _cardList.Count - 1)
            {
                _currentCardSelected = 0;
            }
            else
            {
            _currentCardSelected++;
            }
        }

        InteractCard();
    }

    private void InteractCard()
    {
        var cardTemp = _cardList[_currentCardSelected];
        cardTemp.Interaction();
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
}
