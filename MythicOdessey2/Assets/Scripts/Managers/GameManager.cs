using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public List<EnemyStateMachine> enemies = new List<EnemyStateMachine>();
   //create singelton
    static GameManager _instance;
    public static GameManager Instance => _instance;
    private void Awake(){
        _instance = this;
    }
    private void OnEnable(){
        EventManager.instance.AddAction("CheckEnemies", (object[] args) => {
            if (enemies.Count == 0)
            {
                Debug.Log("Ganaste");
            }
        });
    }
    private void OnDisable(){
        EventManager.instance.RemoveAction("CheckEnemies", (object[] args) => {
            if (enemies.Count == 0)
            {
                Debug.Log("Ganaste");
            }
        });
    }
}
