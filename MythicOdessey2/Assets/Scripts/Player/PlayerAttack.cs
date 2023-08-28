using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class PlayerAttack
{
    [SerializeField] private float _attackSpeed = .5f;
    PlayerManager _playerManager;
    private Animator _anim;
    Transform _transform;
    [SerializeField] private float _attackRadius = 3;
    public float AttackRadius => _attackRadius;

    public bool debugAttack;
    //constructor
    public PlayerAttack(PlayerManager playerManager, Animator animator, Transform transform)
    {
        _playerManager = playerManager;
        _anim = animator;
        _transform = transform;
    }

    public void Update(){
        _anim.speed *= TimeManager.Instance.currentTimeScale;
    }
    

    public IEnumerator SpinAttack(){
        debugAttack = true;
        _playerManager.IsAttacking = true;
        _anim.CrossFade("Pepe_Attack",0.1f);
        var collisions = Physics.OverlapSphere(_transform.position, _attackRadius, 1);
        foreach (var collision in collisions)
        {
            if (collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(1,true);
            }
        }
        yield return new WaitForSeconds(.3f);
        //_anim.SetTrigger("SpinAttack");
        debugAttack = false;
        EventManager.instance.TriggerEvent("OnPlayerAttackFinished");
    }
}
