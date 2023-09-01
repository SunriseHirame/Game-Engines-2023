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
        m_heartsRoot.GetComponentsInChildren(_currentHearts);
        _currentHearts.RemoveAt(0);

        _player = FindFirstObjectByType<PlayerController>();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _currentHearts.Count; i++)
        {
            _currentHearts[i].color = i < _player.CurrentHealth ? m_fullHeartColor : m_lostHeartColor;
        }

        if (_player.CurrentHealth <= 0)
        {
            m_gameOverScreen.SetActive(true);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (EditorApplication.isPlaying) return;

        m_heartsRoot.GetComponentsInChildren(_currentHearts);
        _currentHearts.RemoveAt(0);

        for (int i = 0; i < _currentHearts.Count; i++)
        {
            _currentHearts[i].color = i < _currentHearts.Count - 1 ? m_fullHeartColor : m_lostHeartColor;
        }
    }
#endif
}
