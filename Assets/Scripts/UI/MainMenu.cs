using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string m_firstLevelName;

    [Header("Buttons")]
    [SerializeField] private Button m_selectLevelButton;

    [Header("SubMenus")]
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_selectLevelMenu;

    private void Start()
    {
        // Initially show the main MainMenu view
        ShowMainMenu();
        
        // Bind a listener to the onClick event of the selectLevel button
        // Here we do not need to clean up the reference as we can expect the life time of the button
        // and the MainMenu object to be the same
        m_selectLevelButton.onClick.AddListener(ShowSelectLevelMenu);
    }

    public void Continue ()
    {
        // Load the last level we were playing
        SceneManager.LoadScene(PlayerPrefs.GetString("LastLevel", m_firstLevelName));
    }

    public void ShowMainMenu()
    {
        // Show the main menu ui (set the object active)
        m_mainMenu.SetActive(true);
        // Hide the select menu ui (set the object inactive)
        m_selectLevelMenu.SetActive(false);
    }

    public void ShowSelectLevelMenu()
    {
        // Similar to show menu, but the other way around
        m_mainMenu.SetActive(false);
        m_selectLevelMenu.SetActive(true);
    }

    public void LoadLevel (string levelName)
    {
        // Functionality for loading a level
        SceneManager.LoadScene(levelName);
    }

    // Fixed typo. It was QuiteGame, should have been QuitGame
    // This will break any references set in unity
    // I used rider for refactoring this, rider is smart enough to change the method also in Unity.
    public void QuitGame()
    {
        // We need to use preprocessor if here so that the EditorApplication does not get compiled for the builds
        // compiling it for builds would cause an error as the EditorApplication class exists only in the UnityEditor context
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            // Stop playing in editor
            EditorApplication.isPlaying = false;
        }
#endif
        // Quit a unity game
        Application.Quit();
    }
}
