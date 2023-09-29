using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private Attack[] m_pickup;

    private bool _isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered) return;
        if (!other.CompareTag("Player")) return;

        _isTriggered = true;

        var pickup = m_pickup[Random.Range(0, m_pickup.Length)];

        ReplaceAttackUI.Instance.Show(pickup);
    }
}
