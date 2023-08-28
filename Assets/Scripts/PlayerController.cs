using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_launchForce = 10f;
    [SerializeField] private float m_maxLaunchForce = 30f;

    private bool _launch;
    private float _launchBoostTime;

    public float LaunchForce => Mathf.Clamp (_launchBoostTime * m_launchForce, 0, m_maxLaunchForce);
    public float MaxLaunchForce => m_maxLaunchForce;
    public float NormalizedLaunchForce => LaunchForce / m_maxLaunchForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _launchBoostTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _launch = true;
            Debug.Log("Mouse Button Up");
        }
    }

    private void FixedUpdate()
    {
        if (_launch)
        {
            Debug.Log("Launch Ball");

            var camera = Camera.main;
            var mousePositionOnScreen = camera.ScreenToWorldPoint(Input.mousePosition);
            var ownPosition = transform.position;
            var launchDirection = (ownPosition - mousePositionOnScreen);
            launchDirection.z = 0f;
            launchDirection.Normalize();

            var rigidbody = GetComponent<Rigidbody2D>();
            var launchFoce = launchDirection * m_launchForce * _launchBoostTime;
            launchFoce = Vector3.ClampMagnitude(launchFoce, m_maxLaunchForce);
            rigidbody.AddForce(launchFoce, ForceMode2D.Impulse);

            _launch = false;
            _launchBoostTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            SceneManager.LoadScene((gameObject.scene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Platform") && collision.transform.position.y < transform.position.y)
        {
            if (Camera.main.TryGetComponent<CameraController> (out var cameraController))
                cameraController.SetCameraHeight(collision.transform.position.y);
        }
    }
}
