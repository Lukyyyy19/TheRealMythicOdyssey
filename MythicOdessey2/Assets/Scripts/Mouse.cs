using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class Mouse : MonoBehaviour {
    Vector2 _mousePos;
    float _radius = 0.4f;
    [SerializeField] LayerMask _cardLayer;
    public bool isOnCard;
    Collider2D _cardCollider;
    Transform _selection;
    
    // void Update(){
    //     if(!CardMenuManager.Instance.menuOpen)return;
    //     if (_selection != null)
    //     {
    //         _selection.GetComponent<IInteracteable>().DesInteraction();
    //         _selection = null;
    //     }
    //
    //     _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     _cardCollider = Physics2D.OverlapCircle(_mousePos, _radius, _cardLayer);
    //     if (_cardCollider == null) return;
    //     Debug.Log(_cardCollider.name);
    //     if (_cardCollider.GetComponent<IInteracteable>() == null) return;
    //     _cardCollider.GetComponent<IInteracteable>().Interaction();
    //     _selection = _cardCollider.transform;
    // }
    
    [SerializeField]  GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;
 
   
    void Update()
    {
       
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the game object
        m_PointerEventData.position = Input.mousePosition;
 
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
 
        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
 
        if(results.Count > 0) Debug.Log("Hit " + results[0].gameObject.name);
 
    }

}