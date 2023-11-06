using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class WoodBox : Trap
{
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
        var x = Instantiate(realTrap, worldPosition, quaternion.identity);
        x.woodBox = transform;
        x.gridPosition = gridPosition;
        x.worldPosition = worldPosition;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    // private void OnEnable()
    // {
    //     EventManager.instance.AddAction("OnTrapDestroyed", (x) =>
    //     {
    //             Destroy(gameObject);
    //     });
    // }
    // private void OnDestroy()
    // {
    //     EventManager.instance.RemoveAction("OnTrapDestroyed", (x) =>
    //     {
    //         Destroy(gameObject);
    //     });
    // }
}
