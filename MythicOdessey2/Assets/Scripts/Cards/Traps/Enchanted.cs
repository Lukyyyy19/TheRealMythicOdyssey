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
         Vector3.Distance(x.transform.position, PlayerManager.Instance.transform.position)).First();
      enemy.objective = GameManager.Instance.enemies.First(x => x != enemy).transform;
      enemy.isEnchanted = true;
      Debug.Log(enemy.objective.name);
      Invoke(nameof(ChangeObjectivoToPlayer),3f);
   }

   void ChangeObjectivoToPlayer()
   {
      enemy.objective = PlayerManager.Instance.transform;
      enemy.isEnchanted = false;
   }
}
