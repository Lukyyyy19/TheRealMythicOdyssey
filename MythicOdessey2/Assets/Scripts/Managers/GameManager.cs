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

    public GameObject pauseMenu;
    public bool isPaused;

    private void Awake()
    {
        _instance = this;
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void OnEnable()
    {
        EventManager.instance.AddAction("CheckEnemies", (object[] args) =>
        {
            if (enemies.Count == 0)
            {
                Debug.Log("Ganaste");
            }
        });
    }

    private void OnDisable()
    {
        EventManager.instance.RemoveAction("CheckEnemies", (object[] args) =>
        {
            if (enemies.Count == 0)
            {
                Debug.Log("Ganaste");
            }
        });
    }
}
