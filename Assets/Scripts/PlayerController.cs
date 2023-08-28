using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Player controller is used for controller the ball, aka the player, in the game.
/// In this implementation the player controller has both the input and the action functionalities.
/// In other words the player handles both reading the input from an input devices and then acting based on those.
/// In an ideal scenario there would be separate input/command handling and player logic handling.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // SerializeField is used to mark a field for, well, serialization.
    // In this case it means that the value will be written to disk.
    // The brackets [] are used to denoted an attribute.
    // Attributes are a runtime feature that can be used to add metadata to various things.
    [SerializeField] private float m_launchForce = 10f;
    [SerializeField] private float m_maxLaunchForce = 30f;

    private bool _launch;
    private float _launchBoostTime;

    // These following three are properties. In this case we use them to expose some values to other scripts.
    // All of these properties are implemented in a readonly fashion.
    public float LaunchForce => Mathf.Clamp (_launchBoostTime * m_launchForce, 0, m_maxLaunchForce);
    public float MaxLaunchForce => m_maxLaunchForce;
    public float NormalizedLaunchForce => LaunchForce / m_maxLaunchForce;

    // Start is called before the first frame update
    void Start ()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        // Do to the way unity handles input with the default Input system (used here), we need to read any values in update loop
        // This is because unity updates the default Input systems values just before the update loop, this can cause a fixed update to miss some changes.
        if (Input.GetMouseButton (0))
        {
            _launchBoostTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp (0))
        {
            _launch = true;
            Debug.Log ("Mouse Button Up");
        }
    }

    // FixedUpdate is call at a fixed rate. This is not really true, but do to the way things are handled, you can consider it to be true.
    // FixedUpdate can be call zero, or multiple times per frame. Depending on how long the previous frame was.
    private void FixedUpdate ()
    {
        // In this case as the game is physics based, we handle most of the gameplay logic here.
        // When ever doing things that require physics should be done here.
        if (_launch)
        {
            Debug.Log ("Launch Ball");

            // Used to grab the Camera with "MainCamera" tag. Someone might point out that this is expensive, but that is no longer true as the value is now internally cached.
            var camera = Camera.main;

            // Get the mouse position in world space
            var mousePositionOnScreen = camera.ScreenToWorldPoint (Input.mousePosition);
            var ownPosition = transform.position;
            // Calculate the direction from mouse to player
            var launchDirection = (ownPosition - mousePositionOnScreen);
            // Zero the depth component as out game does not care about that. Gameplay happens on XY plane.
            launchDirection.z = 0f;
            launchDirection.Normalize ();

            // Get the rigidbody component attached to this object
            var rigidbody = GetComponent<Rigidbody2D> ();
            
            // calculate the force to apply to the rigidbody for launching the player
            var launchForce = launchDirection * (m_launchForce * _launchBoostTime);
            
            // Clamp the launch force to a set maximum value
            launchForce = Vector3.ClampMagnitude (launchForce, m_maxLaunchForce);
            
            // Actually apply the force to the rigidbody
            rigidbody.AddForce (launchForce, ForceMode2D.Impulse);

            // reset values
            _launch = false;
            _launchBoostTime = 0;
        }
    }

    // Called when ever this object comes into contact with a Collider2D that is set as Trigger (IsTrigger = true)
    private void OnTriggerEnter2D (Collider2D collision)
    {
        // Check if the object we came in contact with has "Goal" tag
        if (collision.CompareTag ("Goal"))
        {
            // Get next scenes index in build settings.
            // Loop back to zero if the number would be bigger than there are scenes included in build
            var sceneIndexToLoad = (gameObject.scene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
           
            // Load a scene by its build index
            SceneManager.LoadScene (sceneIndexToLoad);
        }
    }

    // Called when ever this object stops colliding (being in contact) with other 2D collider
    private void OnCollisionExit2D (Collision2D collision)
    {
        Debug.Log (collision.gameObject.name);
        // Check if the object had the "Platform" tag
        // Then if so, check if we are about the GameObject in world.
        if (collision.gameObject.CompareTag ("Platform") && collision.transform.position.y < transform.position.y)
        {
            // Try get the CameraController from main camera.
            // If found instruct the camera to move to a new position.
            // "out" keyword tells that the value of the variable is assigned in the method it is passed to
            if (Camera.main.TryGetComponent<CameraController> (out var cameraController))
                cameraController.SetCameraHeight (collision.transform.position.y);
        }
    }
}