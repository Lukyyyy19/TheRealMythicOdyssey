using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrap : Trap {
    private bool _canAttack;
    [SerializeField] float _radius;
    //Crear una clase padre con esto
    
    private void Awake(){
        TestGrid.instance.grid.GetXY(transform.position, out int x, out int y);
        //TestGrid.instance.grid.GetValue(x,y).ResetValue();
    }

    private void Start(){
        StartCoroutine(nameof(ExplodeBomb));
    }

    private void Update(){
        transform.Translate(transform.forward*5*Time.deltaTime);
    }

    IEnumerator ExplodeBomb(){
        yield return new WaitForSeconds(.5f);
        // _canAttack = true;
        Debug.Log("Explotando  1");
        var colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(1, transform);
        }

        yield return new WaitForSeconds(.5f);
        // TestGrid.instance.grid.GetXY(transform.position, out int x1, out int y1);
        // var ssss = TestGrid.instance.grid.MoveObject(Vector2Int.up,new Vector2Int(x1,y1));
        // var worldPosition = TestGrid.instance.grid.GetWorldPosition(ssss.x,ssss.y);
        // transform.position = worldPosition;
        Debug.Log("Explotando  2");
        //transform.Translate(transform.forward*5*Time.deltaTime);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(1, transform);
            //_canAttack = true;
        }

        yield return new WaitForSeconds(.5f);
        // TestGrid.instance.grid.GetXY(transform.position, out int x2, out int y2);
        // var sssss = TestGrid.instance.grid.MoveObject(Vector2Int.up,new Vector2Int(x2,y2));
        // var worldPosition2 = TestGrid.instance.grid.GetWorldPosition(sssss.x,sssss.y);
        // transform.position = worldPosition2;
        //transform.Translate(transform.forward*5*Time.deltaTime);
        Debug.Log("Explotando  3");
        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(1,transform);
        }
        EventManager.instance.TriggerEvent("OnTrapDestroyed",_gridPosition);
        Destroy(gameObject);
        
        //_canAttack = true;
    }
}
