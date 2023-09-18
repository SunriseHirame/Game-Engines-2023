using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Serialization;

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
        ShowMainMenu();
        m_selectLevelButton.onClick.AddListener(ShowSelectLevelMenu);
    }

    public void Continue ()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("LastLevel", m_firstLevelName));
    }

    public void ShowMainMenu()
    {
        m_mainMenu.SetActive(true);
        m_selectLevelMenu.SetActive(false);
    }

    public void ShowSelectLevelMenu()
    {
        m_mainMenu.SetActive(false);
        m_selectLevelMenu.SetActive(true);
    }

    public void LoadLevel (string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuiteGame()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            EditorApplication.isPlaying = false;
        }
#endif
        Application.Quit();
    }
}
