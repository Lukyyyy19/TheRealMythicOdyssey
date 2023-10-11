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
    [SerializeField] private TextMeshPro _xText;
    public Vector2Int gridPosition;
    private void Awake(){
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
        _startColor = _material.color;
        _redColor = new Color32(255, 0, 0, 43);
    }
     public void SetColor(bool red){
         _material.color = red ? _redColor : _startColor;
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
