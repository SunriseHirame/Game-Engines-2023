using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtyThing : MonoBehaviour
{
    // The Hurty angle describes an up facing arc from which this object can damage the player
    [Range (0f, 360f)]
    [SerializeField] private float m_hurtyAngle = 360f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody) return;
        if (!collision.attachedRigidbody.CompareTag("Player")) return;
        if (!collision.attachedRigidbody.TryGetComponent<PlayerController>(out var playerController)) return;

        // We will just accept hits from any direction, no need to do filtering
        if (m_hurtyAngle >= 360f)
        {
            // Makes the player get hurt
            Hurt(playerController);
        }
        else
        {
            // Get the velocity from the players rigidbody.
            // We can assume that the players movement was made in this direction last frame.
            var direction = collision.attachedRigidbody.velocity;
            // Get the angle between the hurt objects down direction and the players movement direction.
            // We want the player to only get hurt when they trigger this object by entering within an angle
            // that is denoted by the m_hurtyAngle.
            // The actual angle from the direction is just half of the arc describes by the m_hurtyAngle
            var angle = Vector2.Angle(-transform.up, direction);
            Debug.Log($"Spikes Collsion Angle: {angle}");

            // Check if the player triggered this from the hurty arc
            if (angle <= m_hurtyAngle / 2f)
            {
                Hurt(playerController);
            }
        }
    }

    private void Hurt(PlayerController player)
    {
        Debug.Log("This should hurt");

        // Calls the hurt method of the player, making them take damage
        player.GetHurt();
    }

    // Allows us to draw some helper graphics into the Scene and Game views
    // Draws only them when this object is selected
    private void OnDrawGizmosSelected()
    {
        // Draws the gizmos with the provided color
        Gizmos.color = Color.yellow;
        // Set the way we should transform points to draw them in world space
        Gizmos.matrix = transform.localToWorldMatrix;
        
        // Draw the arch side lines in local space. The line above will contain information to transform these into world space
        // Here the Vector3.zero means that we start the line from this objects pivot.
        // Quaternion.Euler(0f, 0f, m_hurtyAngle / 2f) will construct an object that allows us to rotate a vector.
        // transform.up is the vector we want to rotate.
        // I highly recommend looking quaternions up from wikipedia.
        // You can also look them up on youtube. 3Blue1Brown has some nice mathematical videos on the subject.  
        Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0f, 0f, m_hurtyAngle / 2f) * transform.up);
        Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0f, 0f, -m_hurtyAngle / 2f) * transform.up);
    }
}
