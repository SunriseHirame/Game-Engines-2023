using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkull : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float m_movementSpeed = 3f;
    [SerializeField] private float m_acceleration = 3f;

    [SerializeField] private int m_startingHelth = 3;

    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = m_startingHelth;
    }

    private void FixedUpdate()
    {
        var player = PlayerController.Instance;
        if (player == null) return;

        var playerPosition = player.transform.position;
        var targetPosition = playerPosition + Vector3.up * 1.5f;

        var directionToPlayer = (targetPosition - transform.position).normalized;
        var desiredVelocity = directionToPlayer * m_movementSpeed;

        m_rigidbody.velocity = Vector3.MoveTowards(m_rigidbody.velocity, desiredVelocity, m_acceleration * Time.deltaTime);
    
        if (m_rigidbody.velocity.magnitude > 0.1f)
        {
            m_rigidbody.rotation = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, -m_rigidbody.velocity, Vector3.up), 0f);
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (!other.gameObject.activeSelf) return;
        if (_currentHealth <= 0) return;

        if (other.CompareTag("Dagger"))
        {
            --_currentHealth;
            other.gameObject.SetActive(false);

            if (_currentHealth <= 0) gameObject.SetActive(false);
        }
    }
}
