using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // RuntimeInitializeOnLoadMethod attribute tells Unity call this method during start up.
    // There are some different timing options selectable by "RuntimeInitializeLoadType"
    // Here we have chosen "BeforeSceneLoad" as we want this script to exist before the first scene is loaded.
    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init ()
    {
        // Create a new game object with name GameManager
        var gameManagerGameObject = new GameObject ("GameManager");
        // Add GameManager component to it
        gameManagerGameObject.AddComponent<GameManager> ();
        // Make so that the gameManagerGameObject will not get destroyed when we switch scenes
        DontDestroyOnLoad (gameManagerGameObject);
    }

    private void Awake ()
    {
        // Hook into a action that gets called when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy ()
    {
        // Unhook from the action. Important for preventing memory leaks and null reference errors
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method will get call by the SceneManager.sceneLoaded action as we have hook this into it
    private static void OnSceneLoaded (Scene scene, LoadSceneMode _)
    {
        Debug.Log ($"OnSceneLoaded {scene.name}");
        // Check if the loaded scene was a level scene
        if (scene.name.StartsWith ("Level"))
        {
            // Store the scene name to player prefs with "LastLevel" as the key
            PlayerPrefs.SetString ("LastLevel", scene.name);
            // Save the player prefs so they persist between sessions.
            // Player Prefs will get saved to disk
            PlayerPrefs.Save ();
        }
    }

    private void Update ()
    {
        // Check if the escape key was pressed and the release.
        // Check if the current scene is NOT "MainMenu"
        if (Input.GetKeyUp (KeyCode.Escape) && SceneManager.GetActiveScene ().name != "MainMenu")
        {
            // Load MainMenu
            SceneManager.LoadScene ("MainMenu");
        }
    }
}