using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Collider[] _enemiesColliders;
    [SerializeField] private EnemyStateMachine _enemy;
    public Vector3 direction;
    [SerializeField] private Vector3 newDirection;
    public EnemyStateMachine CurrentEnemy => _enemy;
    private EnemyStateMachine _auxEnemy;
    private float rad;
    public Action<EnemyStateMachine> OnEnemyChanged;

    private Camera _cam;
    [SerializeField]
    private bool restartPosition;
    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        //  var forward = _cam.transform.forward;
        //  var right = _cam.transform.right;
        //
        // // forward.y = 0f;
        //  right.y = 0f;
        //
        //  forward.Normalize();
        //  right.Normalize();
        //  newDirection = forward * direction.y + right * direction.x;
        //  newDirection = newDirection.normalized;
        newDirection = direction.normalized;
        if (newDirection == Vector3.zero) restartPosition = true;
        if (_enemy != null && restartPosition)
        {
            RaycastHit info;
            if (Physics.SphereCast(_enemy.transform.position, 1f, newDirection, out info, 10, _enemyLayer))
            {
                Debug.Log(info.transform.name);
                var newEn = info.transform.GetComponent<EnemyStateMachine>();
                if (_enemiesColliders.Contains(info.collider))
                {
                    _enemy = newEn;
                    OnEnemyChanged.Invoke(_enemy);
                    restartPosition = false;
                }
            }
        }
    }

    public EnemyStateMachine CheckForEnemies(float radius)
    {
        rad = radius;
        _enemiesColliders = Physics.OverlapSphere(transform.position, radius, _enemyLayer);
        if (_enemiesColliders.Length > 0 && _enemy == null)
        {
            _enemy = _enemiesColliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First()
                .GetComponent<EnemyStateMachine>();
        }
        else if(_enemiesColliders.Length<1)
        {
            _enemy = null;
        }

        if (_enemy != _auxEnemy) OnEnemyChanged?.Invoke(_enemy);
        _auxEnemy = _enemy;
        return _enemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rad);
        // Gizmos.color = Color.red;
        // if (_enemy is not null)
        // {
        //     Gizmos.DrawRay(_enemy.transform.position, newDirection);
        //     Gizmos.DrawSphere(newDirection * 5 + _enemy.transform.position, 1);
        // }
    }
}