using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to move the camera smoothly to new vertical positions
/// </summary>
public class CameraController : MonoBehaviour
{
    // the duration of the smoothing effect
    [SerializeField] private float m_smoothTime = 1f;
    // Additional y position we should apply to any position we request the camera be set to.
    // Bit misleadingly named, but works in the contexts of game.
    [SerializeField] private float m_offsetFromPlatform = 4f;

    private Vector3 _targetPosition;
    private Vector3 _currentVelocity;

    // Used to call immediately when the object is created (Instantiated)
    private void Awake()
    {
        _targetPosition = transform.position;
    }

    // Used to set the cameras height. Additional offset will be applied based on the "m_offsetFromPlatform"
    public void SetCameraHeight (float yPosition)
    {
        var position = transform.position;
        position.y = yPosition + m_offsetFromPlatform;
        _targetPosition = position;
    }

    // Late update is called after updates for this frame have been called on all scripts.
    private void LateUpdate()
    {
        var currentPosition = transform.position;
        // Smooth damp is an easing function that eases into and out of motion.
        // This results in a smooth position change.
        // "ref" keyword is used with _currentVelocity to pass it as reference.
        // Values passed as reference can be modified by the method
        var newPosition = Vector3.SmoothDamp(currentPosition, _targetPosition, ref _currentVelocity, m_smoothTime);
     
        // Apply the smooth position change to the camera
        transform.position = newPosition;
    }
}
