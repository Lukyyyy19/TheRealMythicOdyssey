using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFunctions : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject panel3;
    [SerializeField] private RectTransform groupContainer1;
    [SerializeField] private RectTransform groupContainer2;
    private DeckManager _deckManager;
    
    [SerializeField]private CardUI[] _cardList;
    [SerializeField] private CardUI _currentCard;
    private GameObject _cardGO;
    

    private void Awake()
    {
        _deckManager = GetComponent<DeckManager>();
    }

    private void Start()
    {
        var activeDeck = _deckManager.GetActiveDeck();
        for (int i = 0; i <activeDeck.Count; i++)
        {
            for (int j = 0; j < _cardList.Length; j++)
            {
                if ( activeDeck[i].Id == _cardList[j].ID)
                {
                    _cardList[j].isInDeck = true;
                    _cardList[j].transform.parent = groupContainer2;
                }
            }
        }
    }

    public void InstantiateCard(GameObject go)
    {
        panel.SetActive(false);
        switch (_currentCard.isInDeck)
        {
            case false:
                panel2.SetActive(true);
                break;
            case true:
                panel3.SetActive(true);
                break;
        }
        _cardGO = Instantiate(go);
    }

    public void DestroyCard()
    {
        if (_cardGO)
        {
            Destroy(_cardGO);
            panel2.SetActive(false);
            panel3.SetActive(false);
            panel.SetActive(true);
        }

        _cardGO = null;
    }

    public void CardSelected(CardUI card)
    {
        _currentCard = card;
    }

    public void AddToDeck()
    {
        Debug.Log($"Adding {_currentCard.ID} to deck");
        _deckManager.AddCardToDeck(_currentCard.ID);
        _currentCard.isInDeck = true;
        DestroyCard();
        // foreach (var card in _cardList)
        // {
        //     if (_currentCard.ID == card.ID)
        //     {
        //         card.
        //     }
        // }
        _currentCard.transform.parent = groupContainer2;
        _currentCard = null;
    }

    public void RemoveFromDeck()
    {
        Debug.Log($"Removing {_currentCard.ID} from deck");
        _deckManager.RemoveCardFromDeck(_currentCard.ID);
        _currentCard.isInDeck = false;
        DestroyCard();
        _currentCard.transform.parent = groupContainer1;
        _currentCard = null;
    }
}