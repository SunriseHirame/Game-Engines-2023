using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceAttackUI : MonoBehaviour
{
    public static ReplaceAttackUI Instance { get; private set; }

    [SerializeField] private AttackInfoCard m_newAttack;
    [SerializeField] private AttackInfoCard m_currentLeftAttack;
    [SerializeField] private AttackInfoCard m_currentRightAttack;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        GetComponent<Canvas>().enabled = true;
    }

    public void Show (Attack attack)
    {
        gameObject.SetActive(true);

        m_newAttack.Bind(attack, attack);
        m_currentLeftAttack.Bind(PlayerController.Instance.LeftAttack, attack);
        m_currentRightAttack.Bind(PlayerController.Instance.RightAttack, attack);
    }
}
