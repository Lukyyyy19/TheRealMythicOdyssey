using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.VFX;

public class WoodBox : Trap
{
    [SerializeField]private Trap _cannon;
    [SerializeField]private VisualEffect _smoke;
    private MeshRenderer _mesh;
    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        var newpos = worldPosition + new Vector3(2,0,2);
        transform.position = new Vector3(newpos.x,10,newpos.z);
        transform.DOMoveY(0.5f,.35f).SetEase(Ease.InCirc).OnComplete((() =>
        {
            StartCoroutine("cannonInstantiateRoutine");
        }));
    }

    IEnumerator cannonInstantiateRoutine()
    {
        _smoke.gameObject.SetActive(true);
        _mesh.enabled = false;
        yield return new WaitForSeconds(.5f);
        var x = Instantiate(_cannon, worldPosition, quaternion.identity);
        x.gridPosition = gridPosition;
        x.worldPosition = worldPosition;
        Destroy(gameObject,1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
