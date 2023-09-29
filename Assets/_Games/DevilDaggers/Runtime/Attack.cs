using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="DevilDaggers/Attack")]
public class Attack : ScriptableObject
{
    [SerializeField] private Rigidbody m_daggerPrefab;
    [Min(0.001f)]
    [SerializeField] private float m_fireRate = 20f;
    [Min(1)]
    [SerializeField] private int m_projectileCount = 1;

    [SerializeField] private Vector3 m_daggerSpawnRotationRandomness = new Vector3(5f, 5f, 0f);
    [SerializeField] private float m_launchSpeed = 20f;

    public void OnUpdate(Transform projectileSpawnPoint, bool isUsing, ref float cooldown)
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0f) return;
        if (!isUsing) return;

        cooldown = 1f / m_fireRate;

        for (int i = 0; i < m_projectileCount; i++)
        {
            var randomRotation = Quaternion.Euler(
          Random.Range(-m_daggerSpawnRotationRandomness.x, m_daggerSpawnRotationRandomness.x),
          Random.Range(-m_daggerSpawnRotationRandomness.y, m_daggerSpawnRotationRandomness.y),
          Random.Range(-m_daggerSpawnRotationRandomness.z, m_daggerSpawnRotationRandomness.z));

            var daggerRotation = projectileSpawnPoint.rotation * randomRotation;
            var dagger = Instantiate(m_daggerPrefab, projectileSpawnPoint.position, daggerRotation);
            dagger.velocity = dagger.transform.forward * m_launchSpeed;
        }
     
    }
}
