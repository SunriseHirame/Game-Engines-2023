using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_launchForce = 10f;
    [SerializeField] private float m_maxLaunchForce = 30f;

    [SerializeField] private int m_startingHealth = 3;
    [SerializeField] private ParticleSystem m_hurtParticles;

    private bool _launch;
    private float _launchBoostTime;
    private int _currentHealth;
    public int CurrentHealth => _currentHealth;

    public float LaunchForce => Mathf.Clamp (_launchBoostTime * m_launchForce, 0, m_maxLaunchForce);
    public float MaxLaunchForce => m_maxLaunchForce;
    public float NormalizedLaunchForce => LaunchForce / m_maxLaunchForce;

    private void Awake()
    {
        _currentHealth = m_startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHealth <= 0) return;

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
        if (_currentHealth <= 0) return;

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

    public void GetHurt(int hurtyness = 1)
    {
        if (hurtyness <= 0) return;

        Debug.Log("Got Hurt!");
        m_hurtParticles.Play();
        if (_currentHealth <= 0) return;

        _currentHealth -= hurtyness;

        if (_currentHealth <= 0)
        {
            Debug.Log("Got very hurt");
            //var rigidbody = GetComponent<Rigidbody2D>();
            //rigidbody.isKinematic = true;
            StartCoroutine(RestartCurrentLevel());
        }
    }

    [ContextMenu ("Just Do It")]
    public void JustDoIt()
    {
        GetHurt(_currentHealth);
    }

    private IEnumerator RestartCurrentLevel()
    {
        var handle = SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
        handle.allowSceneActivation = false;

        Time.timeScale = 0.1f;
        Time.fixedDeltaTime *= 0.1f;

        yield return new WaitForSecondsRealtime (20f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        handle.allowSceneActivation = true;
    }
}
