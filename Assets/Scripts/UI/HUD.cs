using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class HUD : MonoBehaviour
{
    [Header("Hearts and stuff")]
    [SerializeField] private Transform m_heartsRoot;
    [SerializeField] private Color m_fullHeartColor;
    [SerializeField] private Color m_lostHeartColor;

    [Header("Game Over")]
    [SerializeField] private GameObject m_gameOverScreen;

    private List<Image> _currentHearts = new List<Image>();

    private PlayerController _player;

    private void Awake()
    {
        // Get all the image components used for the heart display from the parent object
        // Assign them to the _currentHearts list.
        // This is a non allocating variant. you can look this up in the documentation
        m_heartsRoot.GetComponentsInChildren(_currentHearts);
        // Unfortunately when doing the above we also get the "background" graphics from the parent object
        // This will get placed as the first element in the _currentHearts list, so we can just get rid
        // of it by removing the first element from the list.
        _currentHearts.RemoveAt(0);

        // Find the PlayerController from the scene.
        // This can be somewhat expensive, but that does not matter here.
        _player = FindFirstObjectByType<PlayerController>();
    }

    private void LateUpdate()
    {
        // Update the hearts images
        for (int i = 0; i < _currentHearts.Count; i++)
        {
            // Here we use ternary operator to check if the current hart's index in the list is 
            // smaller than the amount of health the player has left.
            // Might by bit confusing to use health and hearts somewhat interchangeably.
            
            // ternary operator -> [condition] ? [if true] : [if false]
            _currentHearts[i].color = i < _player.CurrentHealth ? m_fullHeartColor : m_lostHeartColor;
        }

        // If the players is a goner/wasted show the New Text screen.
        if (_player.CurrentHealth <= 0)
        {
            m_gameOverScreen.SetActive(true);
        }
    }

#if UNITY_EDITOR
    // OnValidate can be used to validate or visuals things
    // Were useful for getting instant feedback on how things will look
    private void OnValidate()
    {
        // We don't want to mess up things if we are playing so do not do anything if we are doing so
        if (EditorApplication.isPlaying) return;

        // Update the available heart objects
        m_heartsRoot.GetComponentsInChildren(_currentHearts);
        // Again remove the background graphic we got from the heart root
        _currentHearts.RemoveAt(0);

        for (int i = 0; i < _currentHearts.Count; i++)
        {
            // Similar as the actual heart update in the LateUpdate
            // here we will have all hearts full, except for the last one
            _currentHearts[i].color = i < _currentHearts.Count - 1 ? m_fullHeartColor : m_lostHeartColor;
        }
    }
#endif
}
