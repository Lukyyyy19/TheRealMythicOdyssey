using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Enchanted : Trap
{
   private EnemyStateMachine enemy;

   private void Start()
   {
      EnchantEnemy();
      EventManager.instance.TriggerEvent("OnTrapDestroyed",gridPosition);
      Destroy(gameObject,3.1f);
   }

   void EnchantEnemy()
   {
     enemy = GameManager.Instance.enemies.OrderBy(x =>
         Vector3.Distance(x.transform.position, worldPosition)).First();
     var obj =  GameManager.Instance.enemies.FirstOrDefault(x => x != enemy)?.transform;
     if (obj != null)
     {
        // enemy.objective = obj;
        // enemy.isEnchanted = true;
        enemy.EnchantEnemy(obj);
        Invoke(nameof(ChangeObjectivoToPlayer),3f);
        
     }
      Debug.Log(enemy.objective.name);
   }

   void ChangeObjectivoToPlayer()
   {
      enemy.DEnchantEnemy();
   }
}
