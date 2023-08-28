using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerMovement {
    // variables for player movement with the name starting with _ are private
   [SerializeField] private float _speed = 5;
    [SerializeField] private float _dashForce = 500;
    [SerializeField] private float _dashTime = .2f;
    private float _turnSpeed = 960f;
    private PlayerManager _playerManager;
    private Rigidbody _rb;
    private Transform _transform;

    private float _timer;
    //crear constructor
    public PlayerMovement(PlayerManager playerManager,Transform transform,Rigidbody rb)
    {
        _playerManager = playerManager;
        _transform = transform;
        _rb = rb;
    }
    private void Move()
    {
        _rb.velocity = Vector3.ClampMagnitude((_playerManager.dir.ToIso() * _playerManager.dir.normalized.magnitude) * _speed * TimeManager.Instance.currentTimeScale, _speed);
    }
    private void PlayerLookOnMovement()
    {
        if (_playerManager.dir == Vector3.zero || _playerManager.IsAttacking) return;
        var rotation = Quaternion.LookRotation(_playerManager.dir.ToIso(), Vector3.up);
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, rotation, _turnSpeed * Time.deltaTime);
    }

    public void Dash(){
        if (_timer < _dashTime)
        {
            _playerManager.IsDahing = true;
            Vector3 forceToApply = _playerManager.dir.ToIso() * _dashForce * TimeManager.Instance.currentTimeScale;
            _rb.velocity = Vector3.zero;
            _rb.AddForce(forceToApply, ForceMode.Impulse);
            
        }

        _timer = 0;
        _playerManager.IsDahing = false;
        _playerManager.RequireNewDashPress = true;
        return;
        // yield return new WaitForSeconds(_dashTime);
        // _playerManager.IsDahing = false;
    }

    public void Update(){
        PlayerLookOnMovement();
        if(_playerManager.IsDahing)
            _timer+= Time.deltaTime;
    }

    public void FixedUpdate(){
        Move();
    }
}
