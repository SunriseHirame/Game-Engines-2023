using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackInfoCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI m_attackNameTitle;
    [SerializeField] private int m_correspondsToMouseButton = -1;

    private Attack _attack;

    public void Bind(Attack attackToDisplayer, Attack attackToReplaceWith)
    {
        _attack = attackToReplaceWith;
        m_attackNameTitle.text = attackToDisplayer.name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked!");

        if (m_correspondsToMouseButton >= 0)
        {
            PlayerController.Instance.SetAttack(_attack, m_correspondsToMouseButton);
        }

        ReplaceAttackUI.Instance.gameObject.SetActive(false);
    }
}
