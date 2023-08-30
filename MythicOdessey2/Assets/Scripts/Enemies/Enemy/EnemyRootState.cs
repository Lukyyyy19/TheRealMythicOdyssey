using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRootState : EnemyBaseState {
    public EnemyRootState(EnemyFactoryState factory, EnemyStateMachine ctx) : base(factory, ctx) {
        IsRootState = true;
    }

    public override void CheckSwitchStates() { }

    public override void EnterState() {
        Debug.Log("asdasdad");
        SetSubState(Factory.Chase());
    }

    public override void FixedUpdateState() { }

    public override void ExitState() { }

    public IEnumerator InitializeSubStates() {
        yield return new WaitUntil(() => Ctx != null);
        SetSubState(Factory.Idle());
    }
}