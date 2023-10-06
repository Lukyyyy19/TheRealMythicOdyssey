using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using TMPro;
public class GameManager : MonoBehaviour
{
    public List<EnemyStateMachine> enemies = new List<EnemyStateMachine>();
    public int enemiesKilled;
    [SerializeField]
    private EnemyStateMachine _enemiesPrefab;

    [SerializeField] private TMP_Text _text;
    //create singelton
    static GameManager _instance;
    public static GameManager Instance => _instance;

    public GameObject pauseMenu;
    public bool isPaused;

    private float _startTime = 2f;
    private float _currentTime;
    [SerializeField]private float _gameTime = 1;
    private void Awake()
    {
        if (_instance == null)
        {
        _instance = this;
        DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        //pauseMenu.SetActive(false);
        _currentTime = _startTime;
    }

    private void Update()
    { 
        if(isPaused)return;
        _gameTime -= Time.deltaTime*TimeManager.Instance.currentTimeScale;
        if(_text)
            _text.text = Mathf.FloorToInt(_gameTime).ToString();
        if(_gameTime<=0)LevelManager.instance.LoadScene("GameOver");
        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime*TimeManager.Instance.currentTimeScale;
        }
        else
        {
            if(enemies.Count<3)
                StartCoroutine(nameof(SpawnEnemy));
            _currentTime = _startTime;
        }
        
        
        
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
        //Time.timeScale = 0f;
        TimeManager.Instance.currentTimeScale = 0f;
        isPaused = true;
        //StopCoroutine(nameof(SpawnEnemy));
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        //Time.timeScale = 1f;
        TimeManager.Instance.currentTimeScale = 1f;
        isPaused = false;
    }

    private void OnEnable()
    {
        EventManager.instance.AddAction("CheckEnemies", (object[] args) =>
        {
            enemiesKilled++;
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
            enemiesKilled++;
            if (enemies.Count == 0)
            {
                Debug.Log("Ganaste");
            }
        });
    }

    IEnumerator SpawnEnemy()
    {
        var x = Random.Range(-15, 15);
        var y = Random.Range(-15, 15);
        Vector3 newPos = new Vector3(x, 0, y);
        while (isPaused)
        {
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        var enemy = Instantiate(_enemiesPrefab, newPos, Quaternion.identity);
    }
}
