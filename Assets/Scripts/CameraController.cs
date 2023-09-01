using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_smoothTime = 1f;
    [SerializeField] private float m_offsetFromPlatform = 4f;

    private Vector3 _targetPosition;
    private Vector3 _currentVelocity;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    public void SetCameraHeight (float yPosition)
    {
        var position = transform.position;
        position.y = yPosition + m_offsetFromPlatform;
        _targetPosition = position;
    }

    private void LateUpdate()
    {
        var currentPosition = transform.position;
        var newPosition = Vector3.SmoothDamp(currentPosition, _targetPosition, ref _currentVelocity, m_smoothTime);
        transform.position = newPosition;
    }
}
