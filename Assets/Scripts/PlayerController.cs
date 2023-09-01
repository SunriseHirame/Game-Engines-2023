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
        // Check iff we got hurt, but the hurt was not significant
        // In other words if we would not take damage, return
        if (hurtyness <= 0) return;

        Debug.Log("Got Hurt!");
        // Player the hurt particles! Blood everywhere!
        m_hurtParticles.Play();
        // Do nothing else if we have already suffered a terrible fate.
        if (_currentHealth <= 0) return;

        // Deduct hp equal to the hurtyness
        _currentHealth -= hurtyness;

        // Check if this was too much for us
        if (_currentHealth <= 0)
        {
            Debug.Log("Got very hurt");
            // Following to lines can be commented out to make the player stop when they die
            //var rigidbody = GetComponent<Rigidbody2D>();
            //rigidbody.isKinematic = true;
            
            // Start to reload the level with a coroutine
            // This allows us to show the New Text screen.
            StartCoroutine(RestartCurrentLevel());
        }
    }

    // Context Menu allows us to show something in the Unity Editor in this components kebab menu.
    [ContextMenu ("Just Do It")]
    public void JustDoIt()
    {
        // Makes the player take damage equal to their remaining health
        GetHurt(_currentHealth);
    }

    private IEnumerator RestartCurrentLevel()
    {
        // Load a new scene asyncronously, allowing us to still keep playing or for us to show some stuff on screen
        var handle = SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
        // Do not activate the scene immediately when it gets loaded
        handle.allowSceneActivation = false;

        // Create a slow motion effect on death
        Time.timeScale = 0.1f;
        // Make the physics smoother during the slow motion
        // This is required as the physics tick rate is based on the in game time
        // Which we have slowed on the previous line
        Time.fixedDeltaTime *= 0.1f;

        // Wait for 20 seconds REAL TIME. Ooops. Maybe we should reduce this again?
        // I guess is good that we no longer use the in game time.
        yield return new WaitForSecondsRealtime (20f);

        // Restore the time scale to 1 which is default
        Time.timeScale = 1f;
        // Restore the fixed time step to 0.02f which is default
        Time.fixedDeltaTime = 0.02f;

        // Finally allow the loaded scene to actually activate.
        // In practice this means the we unload the current scene
        // And active the new scene 
        handle.allowSceneActivation = true;
    }
}
