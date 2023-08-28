using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used as a visualization component for the player controller launch power
/// </summary>
public class LaunchPowerMeter : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private Transform m_scaleRoot;
    [SerializeField] private SpriteRenderer m_barRenderer;

    [SerializeField] private float m_launchForceScaleFactor = 0.1f;

    [SerializeField] private Color m_minColor;
    [SerializeField] private Color m_maxColor;
    [SerializeField] private Gradient m_barColors;

    // Late update is called after updates for this frame have been called on all scripts.
    private void LateUpdate()
    {
        
        // Same code as in PlayerController. This should be refactored.
        // You can find detailed explanation from PlayerController
        var camera = Camera.main;
        var mousePositionOnScreen = camera.ScreenToWorldPoint(Input.mousePosition);
        var ownPosition = transform.position;
        var launchDirection = (ownPosition - mousePositionOnScreen);
        launchDirection.z = 0f;
        launchDirection.Normalize();

        // We do the magic of the launch power bar by just scaling an object
        // First we grab the current scale, so that we preserve the x and z scale
        var currentScale = m_scaleRoot.localScale;
        // The we adjust the why scale
        currentScale.y = m_playerController.LaunchForce * m_launchForceScaleFactor;
        // finally we apply the scale back to the transform
        m_scaleRoot.localScale = currentScale;

        // here we reorient the bar to face into launch direction
        m_scaleRoot.up = launchDirection;


        // Two different ways to interpolate color
        // First we just linearly find a color between two colors. These leads to muddy colors and darkening
        // The third argument should be value in range [0,1]
        var color = Color.Lerp(m_minColor, m_maxColor, m_playerController.NormalizedLaunchForce);
        //m_barRenderer.color = color;
        // Here we use a gradient to sample the color, the value should be in range [0,1]
        // With gradient we have quite decent control of the color blending, resulting in much nicer colors.
        m_barRenderer.color = m_barColors.Evaluate(m_playerController.NormalizedLaunchForce);
    }

    // On validate is called when ever a value in the inspector is called
    // Also when the object becomes selected for the first time
    // This can be used to initialize and validate things in this script.
    // Only works in editor
    private void OnValidate()
    {
        if (!m_playerController) m_playerController = GetComponentInParent<PlayerController>();
    }
}
