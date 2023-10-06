using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

   [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
   private static void Init()
   {
        var gameManagerGameObject = new GameObject("GameManager");
        gameManagerGameObject.AddComponent<GameManager>();
        DontDestroyOnLoad(gameManagerGameObject);
   }

    private void Awake()
    {
#if !UNITY_EDITOR
        Debug.unityLogger.filterLogType = LogType.Error | LogType.Exception;
#endif
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        Debug.Log($"OnSceneLoaded {scene.name}");
        if (scene.name.StartsWith("Level"))
        {
            PlayerPrefs.SetString("LastLevel", scene.name);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
