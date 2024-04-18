using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUI : MonoBehaviour
{
    [SerializeField] private CardsTypeSO CardsTypeSo;
    public int ID => CardsTypeSo.id;
    public bool isInDeck;
}
