using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        LevelManager.instance.LoadScene(sceneName);
        Time.timeScale = 1f;
        Debug.Log($"Scene Changed to:" + sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}
