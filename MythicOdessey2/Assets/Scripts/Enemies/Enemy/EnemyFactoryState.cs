using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyFactoryState
{
    private enum EnemyStates
    {
        Root,
        Idle,
        Chase,
        Attack
    }


    private EnemyStateMachine _ctx;
    private Dictionary<EnemyStates, EnemyBaseState> _statesDic = new Dictionary<EnemyStates, EnemyBaseState>();

    public EnemyFactoryState(EnemyStateMachine ctx) {
        _statesDic.Add(EnemyStates.Root, new EnemyRootState(this,ctx));
        _statesDic.Add(EnemyStates.Idle, new EnemyIdleState(this,ctx));
        _statesDic.Add(EnemyStates.Chase, new EnemyChaseState(this,ctx));
        _statesDic.Add(EnemyStates.Attack, new EnemyAttackState(this,ctx));
    }

    public EnemyBaseState Root() {
        return _statesDic[EnemyStates.Root];
    }

    public EnemyBaseState Idle() {
        return _statesDic[EnemyStates.Idle];
    }

    public EnemyBaseState Chase() {
        return _statesDic[EnemyStates.Chase];
    }

    public EnemyBaseState Attack() {
        return _statesDic[EnemyStates.Attack];
    }

    public void SetContext(EnemyStateMachine ctx) {
        _ctx = ctx;
        foreach (var state in _statesDic.Values) {
           state.SetContext(_ctx);
        }
    }
}