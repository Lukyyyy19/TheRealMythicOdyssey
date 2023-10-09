using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour, IInteracteable
{
    public int value;
    [SerializeField] private Color cardColor;
    private Vector3 _startPos;
    public CardsTypeSO prefab;
    private Animator _animator;
    private Image _image;
    private Color _startColor;
    private Vector3 _startRotation;
    private bool _currentCard;
   [SerializeField] private Transform _ghostCard;
   private bool _startDrag;
    private void Awake(){
        _startPos = transform.localPosition;
        _animator = GetComponent<Animator>();
        _image = GetComponent<Image>();
        _startColor = _image.color;
        _startRotation = transform.localEulerAngles;
    }

    public void DesInteraction(){
        transform.localPosition =
            _startPos; //new Vector3(transform.localPosition.x, transform.localPosition.y - 10, transform.localPosition.z);
        _currentCard = false;
    }

    public void Interaction(){
        
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);
        _currentCard = true;
    }

    public void FollwoCursor()
    {
        _image.color = Color.clear;
         transform.SetParent(CardMenuManager.Instance.cardMenu.transform.parent);
        // Vector3 newPos;
        // newPos.x = Helper.GetMouseWorldPosition().x;
        // newPos.y = Helper.GetMouseWorldPosition().y;
        // newPos.z = -150f;
        _startDrag = true;
        transform.position = Helper.GetMouseWorldPosition();
    }

    private void Update()
    {
        Debug.Log(_startDrag);
        if (_startDrag && _ghostCard)
            _ghostCard.position = transform.position;
    }

    public void Down()
    {
        _ghostCard = Instantiate(prefab.prefabGhost);
    }

    public void Up()
    {
        if(_ghostCard)
            Destroy(_ghostCard.gameObject);
    }

    public void OnEndDarag()
    {
        _startDrag = false;
        if (_currentCard)
        {
            EventManager.instance.TriggerEvent("OnCardTrigger",prefab);
        }
        transform.SetParent(CardMenuManager.Instance.cardMenu.transform);
        Up();
        _image.color = _startColor;
    }

    private void OnEnable(){
        EventManager.instance.AddAction("OnCantBuild", (x) => {
            //if(!_currentCard)return;
            _animator.CrossFade("Shake",.1f);
            StartCoroutine(nameof(Shake));
            _currentCard = false;
            
        });
        EventManager.instance.AddAction("OnOpenMenu", (x) => {
            transform.localEulerAngles = _startRotation;
            DesInteraction();
            transform.SetParent(CardMenuManager.Instance.cardMenu.transform);
            Up();
            _image.color = _startColor;
        });
    }

    private void OnDisable()
    {
        EventManager.instance.RemoveAction("OnCantBuild", (x) => {
            if(!_currentCard)return;
            _animator.CrossFade("Shake",.1f);
            StartCoroutine(nameof(Shake));
            _currentCard = false;
        });
        EventManager.instance.RemoveAction("OnOpenMenu", (x) => {
            transform.localEulerAngles = _startRotation;
            DesInteraction();
            transform.SetParent(CardMenuManager.Instance.cardMenu.transform);
            Up();
            _image.color = _startColor;
        });
    }

    IEnumerator Shake(){
        _image.color = Color.red;
        yield return new WaitForSeconds(.1f);
        _image.color = Color.white;
        yield return new WaitForSeconds(.1f);
        _image.color = _startColor;
    }
    
    // private void Start()
    // {
    //     text.text = value.ToString();
    // }
    // private void OnMouseDown()
    // {
    //     if (!GameManager.instance.canPlay)
    //     {
    //         if (GameManager.instance.isTutorial && Tutorial.tutorialsComplete > 0 || !GameManager.instance.isTutorial)
    //         {
    //             GameManager.instance.OnCardSelected(cardColor, value, this.gameObject);
    //             //RandomAnimation.playAnim = true;
    //             UI_Manager.instnace.UpdateText(value.ToString());
    //         }
    //     }
    // }
}
