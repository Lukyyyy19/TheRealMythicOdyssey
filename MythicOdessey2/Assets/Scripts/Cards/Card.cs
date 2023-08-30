using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Card : MonoBehaviour, IInteracteable
{
    public int value;
    [SerializeField] private Color cardColor;
    private Vector3 _startPos;
    public CardsTypeSO prefab;
    private void Awake(){
        _startPos = transform.localPosition;
    }

    public void DesInteraction(){
        transform.localPosition =
            _startPos; //new Vector3(transform.localPosition.x, transform.localPosition.y - 10, transform.localPosition.z);

    }

    public void Interaction()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);

    }

    public void FollwoCursor(){
        Vector3 newPos;
        newPos.x = Helper.GetMouseWorldPosition().x;
        newPos.y = Helper.GetMouseWorldPosition().y;
        newPos.z = -150f;
        
        transform.position = Helper.GetMouseWorldPosition();
    }

    public void OnEndDarag(){
        EventManager.instance.TriggerEvent("OnCardTrigger",prefab);
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
