using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPowerMeter : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private Transform m_scaleRoot;
    [SerializeField] private SpriteRenderer m_barRenderer;

    [SerializeField] private float m_launchForceScaleFactor = 0.1f;

    [SerializeField] private Color m_minColor;
    [SerializeField] private Color m_maxColor;
    [SerializeField] private Gradient m_barColors;

    private void LateUpdate()
    {
        var camera = Camera.main;
        var mousePositionOnScreen = camera.ScreenToWorldPoint(Input.mousePosition);
        var ownPosition = transform.position;
        var launchDirection = (ownPosition - mousePositionOnScreen);
        launchDirection.z = 0f;
        launchDirection.Normalize();


        var currentScale = m_scaleRoot.localScale;
        currentScale.y = m_playerController.LaunchForce * m_launchForceScaleFactor;
        m_scaleRoot.localScale = currentScale;

        m_scaleRoot.up = launchDirection;


        var color = Color.Lerp(m_minColor, m_maxColor, m_playerController.NormalizedLaunchForce);
        //m_barRenderer.color = color;
        m_barRenderer.color = m_barColors.Evaluate(m_playerController.NormalizedLaunchForce);
    }

    private void OnValidate()
    {
        if (!m_playerController) m_playerController = GetComponentInParent<PlayerController>();
    }
}
