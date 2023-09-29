using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlane : MonoBehaviour {
    private MeshRenderer _meshRenderer;
    private Material _material;
    private Color _startColor;
    private Color _redColor;
    private void Awake(){
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
        _startColor = _material.color;
        _redColor = new Color32(255, 0, 0, 43);
    }
     public void SetColor(bool red){
         _material.color = red ? _redColor : _startColor;
     }
    
}
