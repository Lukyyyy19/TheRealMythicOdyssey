using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardUI_IG_Container : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numbers;
    [SerializeField] private Transform container2;

    private void Awake()
    {
        var deck = DeckManager.instance.GetActiveDeck();
        for (var index = 0; index < deck.Count; index++)
        {
            var card = deck[index];
            Instantiate(card, transform);
            var n = Instantiate(numbers, container2);
            n.name = (index+1).ToString();
            n.text = (index+1).ToString();
        }
    }
}