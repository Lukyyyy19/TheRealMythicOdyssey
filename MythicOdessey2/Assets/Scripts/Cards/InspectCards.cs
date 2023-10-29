using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class InspectCards : MonoBehaviour
{
    private Transform _child;
    private bool _cardInspected;
    [SerializeField]private GameObject _button;
    private Vector3 _startPos;
    private void Awake()
    {
        _child = GetComponentInChildren<Transform>();
        _startPos = transform.position;
    }

    private void OnMouseDown()
    {
        transform.DOMove(Vector3.zero, .75f);
        _cardInspected = true;
        EventManager.instance.TriggerEvent("OnCardInspected");
        _button.SetActive(true);
    }

    private void OnEnable()
    {
        EventManager.instance.AddAction("OnCardInspected",(x)=>
        {
            if(!_cardInspected)
            _child.gameObject.SetActive(false);
        });
        
        EventManager.instance.AddAction("OnCardDesinspected",(x)=>
        {
            if(_cardInspected)
                transform.DOMove(_startPos, .75f);
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
