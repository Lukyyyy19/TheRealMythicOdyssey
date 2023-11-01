using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class InspectCards : MonoBehaviour
{
    [SerializeField]private Transform _child;
    [SerializeField]private Collider _childCol;
    [SerializeField]private Collider _thisCol;
    private bool _cardInspected;
    [SerializeField]private GameObject _button;
    private Vector3 _startPos;
    private Vector3 _midPos;
    [SerializeField] private GameObject _textChild;
    RotateCard _rotateCardScript;
    private void Awake()
    {
        _startPos = transform.position;
        _thisCol = GetComponent<Collider>();
        _midPos = new Vector3( 0.01f,0, 0);
        _rotateCardScript = _child.GetComponent<RotateCard>();
    }

    private void OnMouseDown()
    {
        _childCol.enabled = true;
        _thisCol.enabled = false;
        transform.DOMove(_midPos, .75f);
        transform.DOScale(2, .75f);
        _cardInspected = true;
        _rotateCardScript.enabled = true;
        EventManager.instance.TriggerEvent("OnCardInspected");
        _button.SetActive(true);
        _textChild.SetActive(true);
    }

    private void OnEnable()
    {
        EventManager.instance.AddAction("OnCardInspected",(x)=>
        {
            _thisCol.enabled = false;
            if(!_cardInspected)
            _child.gameObject.SetActive(false);
        });
        
        EventManager.instance.AddAction("OnCardDesinspected",(x)=>
        {
            _rotateCardScript.enabled = false;
            _textChild.SetActive(false);
            _thisCol.enabled = true;
            _childCol.enabled = false;
            if (_cardInspected)
            {
                transform.DOMove(_startPos, .75f);
                transform.DOScale(1, .75f);
                _child.DORotate(new Vector3(0, 180, 0), .5f);
            }

            _cardInspected = false;
            _child.gameObject.SetActive(true);
        });
    }

    private void OnDisable()
    {
        EventManager.instance.RemoveAction("OnCardInspected",(x)=>
        {
            if(!_cardInspected)
                _child.gameObject.SetActive(false);
        });
        EventManager.instance.RemoveAction("OnCardDesinspected",(x)=> _child.gameObject.SetActive(true));
    }
}
