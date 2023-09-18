using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_movingObject;
    [SerializeField] private float m_speed = 3f;
    [SerializeField] private Transform[] m_waypoints;

    private int _targetWaypointIndex;

    private void FixedUpdate()
    {
        _targetWaypointIndex %= m_waypoints.Length;
        var target = m_waypoints[_targetWaypointIndex];

        var newPosition = Vector2.MoveTowards(m_movingObject.position, target.position, Time.deltaTime * m_speed);
        m_movingObject.MovePosition(newPosition);

        if (Vector2.Distance(newPosition, target.position) < 0.1f)
        {
            ++_targetWaypointIndex;
        }
    }

    private void OnDrawGizmos()
    {
        if (m_waypoints.Length >= 2)
        {
            for (int i = 1; i < m_waypoints.Length; i++)
            {
                Gizmos.DrawLine(m_waypoints[i - 1].position, m_waypoints[i].position);
            }
        }
    }
}
