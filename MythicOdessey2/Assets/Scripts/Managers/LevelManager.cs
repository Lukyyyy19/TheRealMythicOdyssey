using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _loadingBar;
    private float _target;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public async void LoadScene(string sceneName)
    {
        _target = 0;
        _loadingBar.fillAmount = 0;
        
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        
        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
                _target = scene.progress;
        } while (scene.progress < 0.9f);
        
        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
    }

    private void Update()
    {
        _loadingBar.fillAmount = Mathf.MoveTowards(_loadingBar.fillAmount, _target, 3 * Time.deltaTime);
    }

}
