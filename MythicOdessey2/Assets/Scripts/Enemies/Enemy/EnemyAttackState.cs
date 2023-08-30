using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyFactoryState factory, EnemyStateMachine ctx) : base(factory,ctx) { }

    public override void EnterState() {
        Debug.Log("Attack");
        Ctx.Anim.CrossFade("Attack_3Combo_1 0", .2f);
    }
    
    public override void UpdateState(){
        base.UpdateState();
        Ctx.LookAtPlayer();
    }

    public override void FixedUpdateState() {
        
    }
    

    public override void ExitState() {
        
    }

    public override void CheckSwitchStates() {
        if (!Ctx.IsPlayerInRange) SwitchState(Factory.Idle());
        if (!Ctx.IsPlayerInAttackRange) SwitchState(Factory.Chase());
    }
    
    
}