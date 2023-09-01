using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    // The object we want to move. We want it to interact with physics so we force it to be a rigidbody
    [SerializeField] private Rigidbody2D m_movingObject;

    // The speed at which we move
    [SerializeField] private float m_speed = 3f;

    // An array or "collection" of waypoints we want to move through.
    [SerializeField] private Transform[] m_waypoints;

    // the index of current target waypoint int he m_waypoints array
    private int _targetWaypointIndex;

    private void FixedUpdate ()
    {
        // Make sure the _targetWayPointIndex does not go over the m_waypoints array
        _targetWaypointIndex %= m_waypoints.Length;

        // get the actual target object from the array
        var target = m_waypoints[_targetWaypointIndex];

        // Calculate new position for the object
        var newPosition = Vector2.MoveTowards (m_movingObject.position, target.position, Time.deltaTime * m_speed);

        // Actually move the object
        m_movingObject.MovePosition (newPosition);

        // Check if we are close enough to the point, so that we can consider us having reached the destination
        if (Vector2.Distance (newPosition, target.position) < 0.1f)
        {
            ++_targetWaypointIndex;
        }
    }

    // Allows us to draw some helper graphics into the Scene and Game views
    // This will always draw the gizmos
    private void OnDrawGizmos ()
    {
        // Only draw lines between waypoints if there are two or more of them
        if (m_waypoints.Length >= 2)
        {
            // loop through the waypoints starting from the index 1.
            // We skip the index 0 in the loop, because we will draw the lines from previous to current waypoint
            for (int i = 1; i < m_waypoints.Length; i++)
            {
                Gizmos.DrawLine (m_waypoints[i - 1].position, m_waypoints[i].position);
            }
        }
    }
}