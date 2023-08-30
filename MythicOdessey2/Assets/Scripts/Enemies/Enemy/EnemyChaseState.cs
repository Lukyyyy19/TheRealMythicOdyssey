using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState {
    public EnemyChaseState(EnemyFactoryState factory, EnemyStateMachine ctx) : base(factory, ctx){
    }

    public override void CheckSwitchStates(){
        //if (!Ctx.IsPlayerInRange || Ctx.StopChasing) SwitchState(Factory.Idle());
        //if (Ctx.IsPlayerInAttackRange && Ctx.IsEnemyWithSword) SwitchState(Factory.Attack());
    }

    public override void EnterState(){
        Debug.Log("Chase Ste");
       // if (Ctx.Anim) Ctx.Anim.CrossFade("Walk_ver_B_Front 0", .2f);
    }

    public override void UpdateState(){
        base.UpdateState();
       // if(Ctx.CanLookAtPlayer) Ctx.LookAtPlayer();
        FollowPlayer();
    }

    public override void FixedUpdateState(){
    }

    public override void ExitState(){
    }

    public void FollowPlayer(){
        // Ctx.transform.position = Vector3.MoveTowards(Ctx.transform.position, PlayerManager.instance.transform.position,
        //     Ctx.Speed * Time.deltaTime);
        //Ctx.Rb.velocity = (PlayerManager.Instance.transform.position - Ctx.transform.position) * Ctx.Speed * Time.fixedDeltaTime;
        Ctx.NavMeshAgent.destination = PlayerManager.Instance.transform.position;
    }
}