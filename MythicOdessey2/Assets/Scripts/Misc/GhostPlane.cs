using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GhostPlane : MonoBehaviour {
    private MeshRenderer _meshRenderer;
    private Material _material;
    private Color _startColor;
    private Color _redColor;
    private Color _blueColor;
    [SerializeField] private TextMeshPro _xText;
    public Vector2Int gridPosition;
   [SerializeField] private bool changeColor;
    private void Awake(){
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
        _startColor = _material.color;
        _redColor = new Color32(255, 0, 0, 43);
        _blueColor = new Color32(0, 0, 255, 43);
        if (changeColor)
            _startColor = _blueColor;
    }
     public void SetColor(int color){
         //_material.color = red ? _redColor : _startColor;
         switch (color)
         {
             case 0:
                 _material.color = _startColor;
                 return;
             case 1:
                 _material.color = _redColor;
                 return;
             case 2:
                 _material.color = _blueColor;
                 return;
         }
     }

     private void OnEnable()
     {
         EventManager.instance.AddAction("OnCantBuild", (x) =>
         {
             if ((Vector2Int)x[0] == gridPosition)
             {
                 StartCoroutine(nameof(ColorChangeX));
             }
         });
     }

     IEnumerator ColorChangeX()
     {
         _xText.enabled = true;
         _xText.color = Color.red;
         yield return new WaitForSeconds(.1f);
         _xText.color = Color.white;
         yield return new WaitForSeconds(.1f);
         _xText.enabled = false;
     }
}
