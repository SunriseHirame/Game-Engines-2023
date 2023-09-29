using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_enemyToSpawn;
    [SerializeField] private float m_spawnRadius = 25f;
    [SerializeField] private float m_spawnRate = 1f;
    [SerializeField] private float m_spawnStartDelay = 3f;

    private float _timeUntillNextSpawn;

    private void Awake()
    {
        _timeUntillNextSpawn = m_spawnStartDelay;
    }

    private void Update()
    {
        _timeUntillNextSpawn -= Time.deltaTime;
        if (m_spawnRate > 0f && _timeUntillNextSpawn <= 0f)
        {
            _timeUntillNextSpawn = 1f / m_spawnRate;

            var rotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);
            var spawnPosition = transform.position + rotation * (transform.forward * m_spawnRadius);
            var enemy = Instantiate(m_enemyToSpawn, spawnPosition, rotation * Quaternion.Euler(0f, 180f, 0f));
        }
    }
}
