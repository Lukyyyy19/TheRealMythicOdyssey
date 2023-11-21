using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[Serializable]
public class PlayerAttack {
    [SerializeField] private float _attackSpeed = .5f;
    PlayerManager _playerManager;
    private Animator _anim;
    Transform _transform;
    [SerializeField] private float _attackRadius = 6;
    public float AttackRadius => _attackRadius;

    public bool debugAttack;

    private GameObject _hitVfx;
    //constructor
    public PlayerAttack(PlayerManager playerManager, Animator animator, Transform transform,GameObject hitVfx){
        _playerManager = playerManager;
        _anim = animator;
        _transform = transform;
        _hitVfx = hitVfx;
    }

    public void Update(){
        _anim.speed *= TimeManager.Instance.currentTimeScale;
        // var collisions = Physics.OverlapSphere(_transform.position, _attackRadius);
        // foreach (var collision in collisions)
        // {
        //     if (collision.CompareTag("Player")) continue;
        //     collision.GetComponent<IDamageable>()?.TakeDamage(0,_playerManager.transform);
        // }

    }
    
    public IEnumerator SpinAttack()
    {
        if (CardMenuManager.Instance.menuOpen) yield return null;
        _playerManager.RootMotion = false;
        debugAttack = true;
        _playerManager.IsAttacking = true;
        _anim.CrossFade("Pepe_Attack", 0.1f);
        //_transform.DORotate(Vector3.up * 359, .3f).OnComplete(()=>_transform.rotation = Quaternion.identity);
        var collisions = Physics.OverlapSphere(_transform.position, _attackRadius);
        Debug.Log(collisions.Length);
        Collider _exCol = null;
        foreach (var collision in collisions)
        {
            if(_exCol == collision)continue;
            if (collision.CompareTag("Player")) continue;
            if(!collision.TryGetComponent(out IDamageable damageable))continue;
            GameObject spawn = null;
            if (spawn == null)
            {
                spawn = MonoBehaviour.Instantiate(_hitVfx, collision.transform.position, Quaternion.identity);
                MonoBehaviour.Destroy(spawn,.5f);
            }
            damageable.TakeDamage(0,_playerManager.transform);
            _exCol = collision;
        }
        
        yield return new WaitForSeconds(.3f);
        _playerManager.IsAttacking = false;
        _playerManager.RootMotion = true;
        EventManager.instance.TriggerEvent("OnPlayerAttackFinished");
       
    }
}