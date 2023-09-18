using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtyThing : MonoBehaviour
{
    [Range (0f, 360f)]
    [SerializeField] private float m_hurtyAngle = 360f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody) return;
        if (!collision.attachedRigidbody.CompareTag("Player")) return;
        if (!collision.attachedRigidbody.TryGetComponent<PlayerController>(out var playerController)) return;

        if (m_hurtyAngle >= 360f)
        {
            Hurt(playerController);
        }
        else
        {
            var direction = collision.attachedRigidbody.velocity;
            var angle = Vector2.Angle(-transform.up, direction);
            Debug.Log($"Spikes Collsion Angle: {angle}");

            if (angle <= m_hurtyAngle / 2f)
            {
                Hurt(playerController);
            }
        }
    }

    private void Hurt(PlayerController player)
    {
        Debug.Log("This should hurt");

        player.GetHurt();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0f, 0f, m_hurtyAngle / 2f) * transform.up);
        Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0f, 0f, -m_hurtyAngle / 2f) * transform.up);
    }
}
