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
    [SerializeField] private GameObject _cardChild;
    private Vector2Int _mousepos;
    [SerializeField]private int _id;
    [SerializeField] private bool _isSword;
    public bool canInteract = true;
    public int Id => _id;

    public bool IsSword => _isSword;
    private void Awake()
    {
        _startPos = transform.localPosition;
        _animator = GetComponent<Animator>();
        _image = GetComponent<Image>();
        //startColor = _image.color;
        _startRotation = transform.localEulerAngles;
        CardMenuManager.Instance.AddCard(this);
        canInteract = true;
    }


    private void Start()
    {
    }

    public void DesInteraction()
    {
        transform.localPosition = _startPos; //new Vector3(transform.localPosition.x, transform.localPosition.y - 10, transform.localPosition.z);
        _currentCard = false;
    }

    public void Interaction(){
        if(!canInteract)return;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);
        _currentCard = true;
    }

    private Vector2Int _lastMousePos;
    public void FollwoCursor()
    {
        _cardChild.SetActive(false);
        //_image.color = Color.clear;
         transform.SetParent(CardMenuManager.Instance.cardMenu.transform.parent);
        // Vector3 newPos;
        // newPos.x = Helper.GetMouseWorldPosition().x;
        // newPos.y = Helper.GetMouseWorldPosition().y;
        // newPos.z = -150f;
        // TestGrid.instance.grid.GetXY(Helper.GetMouseWorldPosition(), out int x, out int z);
        // _mousepos.x = x;
        // _mousepos.y = z;
        // TestGrid.instance.UpdateGhostPlaneColors(new Vector2Int(x,z),2);
        // if (_lastMousePos != _mousepos)
        // {
        //     if (TestGrid.instance.grid.GetValue(_lastMousePos.x, _lastMousePos.y)!= null)
        //     {
        //         TestGrid.instance.UpdateGhostPlaneColors(_lastMousePos, 1);
        //     }
        //     else
        //     {
        //         TestGrid.instance.UpdateGhostPlaneColors(_lastMousePos, 0);
        //         
        //     }
        // }
        _startDrag = true;
        transform.position = Helper.GetMouseWorldPosition();
        //_lastMousePos = _mousepos;
    }

    private void Update()
    {
        if (_startDrag && _ghostCard)
            _ghostCard.position = transform.position;
    }

    public void Down()
    {
        if(_isSword)return;
        _ghostCard = Instantiate(prefab.prefabGhost);
    }

    public void Up()
    {
        if(_ghostCard)
            Destroy(_ghostCard.gameObject);
    }

    public void OnEndDarag()
    {
        _cardChild.SetActive(true);
        // _image.color = _startColor;
        _startDrag = false;
        TriggerInstantiateEvent();

        transform.SetParent(CardMenuManager.Instance.cardMenu.transform);
        Up();
    }

    public void TriggerInstantiateEvent()
    {
        if(_isSword)return;
        if (_currentCard)
        {
            EventManager.instance.TriggerEvent("OnCardTrigger",prefab);
        }
        //transform.SetParent(CardMenuManager.Instance.cardMenu.transform);
        //Up();
    }

    // private void OnEnable(){
    //     EventManager.instance.AddAction("OnCantBuild", (x) =>
    //     {
    //         if (!_currentCard) return;
    //         _animator.CrossFade("Shake",.1f);
    //         StartCoroutine(nameof(Shake));
    //         _currentCard = false;
    //     });
    //     EventManager.instance.AddAction("OnOpenMenu", (x) => {
    //         transform.localEulerAngles = _startRotation;
    //         DesInteraction();
    //         transform.SetParent(CardMenuManager.Instance.cardMenu.transform);
    //         Up();
    //         _image.color = _startColor;
    //     });
    // }
    //
    // private void OnDisable()
    // {
    //     EventManager.instance.RemoveAction("OnCantBuild", (x) => {
    //         if(!_currentCard)return;
    //         _animator.CrossFade("Shake",.1f);
    //         StartCoroutine(nameof(Shake));
    //         _currentCard = false;
    //     });
    //     EventManager.instance.RemoveAction("OnOpenMenu", (x) => {
    //         transform.localEulerAngles = _startRotation;
    //         DesInteraction();
    //         transform.SetParent(CardMenuManager.Instance.cardMenu.transform);
    //         Up();
    //         _image.color = _startColor;
    //     });
    // }

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
