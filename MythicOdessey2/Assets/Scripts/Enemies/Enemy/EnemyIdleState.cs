using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyFactoryState factory, EnemyStateMachine ctx) : base(factory,ctx) { }

    public override void CheckSwitchStates() {
        if (Ctx.IsPlayerInRange) SwitchState(Factory.Chase());
       // if (Ctx.IsPlayerInAttackRange && Ctx.IsEnemyWithSword) SwitchState(Factory.Attack());
    }
    public override void EnterState() {
        Debug.Log("Idle");
        //if (Ctx.Anim) Ctx.Anim.CrossFade("Idle_ver_B", .2f);
    }

    public override void UpdateState(){
        base.UpdateState();
        Ctx.LookAtPlayer();
        
    }

    public override void FixedUpdateState() {
        
    }

    public override void ExitState() {
        
    }
}