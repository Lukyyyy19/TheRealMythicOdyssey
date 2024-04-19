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
    //public GameObject cardMenu;
    public bool menuClose;
    [SerializeField] private List<Card> _cardList;
    public static CardMenuManager Instance => _instance;
    [SerializeField]private int _currentCardSelected;
    private bool _isOrdered;
    private bool _isDragging;
    
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _currentCardSelected = _cardList.Count - 1;
       OpenMenu(true);
    }

    private void Update()
    {
        ChangeSelected();
       // CardInteractionOnClick();
    }

    private void CardInteractionOnClick()
    {
        if(cardTemp.IsSword)return;
        if (Input.GetButton("Fire1"))
        {
            _isDragging = true;
            if (cardTemp) cardTemp.transform.position = Helper.GetMouseWorldPosition();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            _isDragging = false;
            if (cardTemp)
            {
                Debug.Log('a');
                TriggerCard();
                return;
            }
        }
    }

    public void TriggerCard()
    {
        cardTemp.TriggerInstantiateEvent();
        cardTemp.DesInteraction();
        CurrentCardSelectedInteraction(_cardList.Count-1,false);
    }

    public void OpenMenu(bool open)
    {
        // TimeManager.Instance.currentTimeScale = open ? 0.25f : 1;
        // EventManager.instance.TriggerEvent("OnTimeChanged",TimeManager.Instance.currentTimeScale);
        // EventManager.instance.TriggerEvent("OnOpenMenu",open);
       // cardMenu.SetActive(open);

        if (!_isOrdered)
        {
            //_cardList = _cardList.OrderBy(x => x.Id).ToList();
            // Card temp = null;
            // for (int i = 0; i < _cardList.Count; i++)
            // {
            //     if (_cardList[i].Id == 6)
            //     {
            //         int x = i;
            //         temp = _cardList[i];
            //     }
            //
            //     if (i == _cardList.Count - 1)
            //     {
            //         _cardList[0] = _cardList[i];
            //         _cardList[i] = temp;
            //     }
            // }
            _isOrdered = true;
        }
        //TimeManager.Instance.currentTimeScale = open ? 0.25f : 1;
        // EventManager.instance.TriggerEvent("OnTimeChanged", TimeManager.Instance.currentTimeScale);
        
        //cardMenu.SetActive(open);
        menuOpen = open;
        if (menuOpen)
        {
                // if(_currentCardSelected!=5)
                //     _currentCardSelected = 5;
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
    
    // public void CloseMenu(bool close)
    // {
    //     cardMenu.SetActive(close);
    // }
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
        if(!PlayerManager.Instance.HasMana)return;
        // Vector3 mousePos = Input.mousePosition;
        // mousePos.z = 100f;
        // var pos = uiCam.ScreenToWorldPoint(mousePos);
        // Debug.DrawRay(uiCam.transform.position,pos-uiCam.transform.position,Color.green);
        // var ray = uiCam.ScreenPointToRay(mousePos);
        // if (Physics.Raycast(ray,out RaycastHit hit, float.MaxValue, cad))
        // {
        //     var card = hit.transform.GetComponentInParent<Card>();
        //     if(card)
        //         CurrentCardSelectedInteraction(card.Id,true);
        // }
        // else if(_isDragging == false)
        // {
        //         CurrentCardSelectedInteraction(5,false);
        // }
        //
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
           CurrentCardSelectedInteraction(_cardList.Count-1,false);
        }
    }

    public LayerMask cad;
    public Camera uiCam; 
    private void CurrentCardSelectedInteraction(int position,bool hands)
    {
        if (_currentCardSelected == position || position+1>_cardList.Count) return;
        //PlayerManager.Instance.HasHandsOccupied = hands;
        // if(hands)EventManager.instance.TriggerEvent("OnOpenMenu", true);
        // else
        // {
        //     EventManager.instance.TriggerEvent("OnOpenMenu", false);
        // }
        _currentCardSelected = position;
        InteractCard();
    }

    public Card cardTemp;
    private void InteractCard()
    {
        
        cardTemp = _cardList[_currentCardSelected];
        cardTemp.Interaction();
        //     CardMenuManager.Instance.GetCurrentCardSelected().TriggerInstantiateEvent();
        //cardTemp.TriggerInstantiateEvent();
        foreach (var card in _cardList)
        {
            if (card != cardTemp)
            {
                card.DesInteraction();
            }
        }
    }

    public void CheckManaCost(float mana)
    {
        foreach (var card in _cardList)
        {
            if(card.IsSword)continue;
            var sr = card.GetComponentInChildren<SpriteRenderer>();
            if (card.prefab.manaCost > mana)
            {
                sr.color = Color.gray;
                card.canInteract = false;
            }
            else
            {
                card.canInteract = true;
                sr.color = Color.white;
            }
        }
    }
    
    public Card GetCurrentCardSelected()
    {
        return _cardList[_currentCardSelected];
    }

    private void OnEnable()
    {
        EventManager.instance.AddAction("OnCardBuilt",(x)=>CurrentCardSelectedInteraction(_cardList.Count-1,false));
        // EventManager.instance.AddAction("HideCardPanel",(x)=>cardMenu.SetActive(false));
        // EventManager.instance.AddAction("ShowCardPanel",(x)=>cardMenu.SetActive(true));
    }
}
