using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private CapsuleCollider m_playerCollider;
    [SerializeField] private float m_moveSpeed = 5f;

    [SerializeField] private Transform m_playerCamera;
    [SerializeField] private float m_rotationSpeed = 360f;
    [SerializeField] private float m_lookUpAndDownSpeed = 20f;

    [Header("Daggers")]
    [SerializeField] private Transform m_daggerSpawnPoint;
    [SerializeField] private Attack m_leftMouseButtonAttack;
    [SerializeField] private Attack m_rightMouseButtonAttack;

    public Attack LeftAttack => m_leftMouseButtonAttack;
    public Attack RightAttack => m_rightMouseButtonAttack;

    private float _leftAttackCooldown;
    private float _rightAttackCooldown;


    private Vector2 _movementInput;
    private Vector2 _lookInput;

    private float _daggerCooldown;

    private bool _isGrounded;
    private float _fallingSpeed;
    private Vector3 _groundNormal;

    public void SetAttack (Attack attack, int mouseButton)
    {
        Debug.Log($"Switch Attack: {attack} {mouseButton}");
        switch (mouseButton)
        {
            case 0:
                m_leftMouseButtonAttack = attack;
                break;
            case 1:
                m_rightMouseButtonAttack = attack;
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _movementInput = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        _lookInput = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }

        _isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, out var hit, 1.1f);
        if (_isGrounded)
        {
            _fallingSpeed = Physics.gravity.y * Time.deltaTime;
            if (hit.distance < 1f) transform.position = hit.point;
            _groundNormal = hit.normal;
        }
        else
        {
            _fallingSpeed += Physics.gravity.y * Time.deltaTime;
            _groundNormal = Vector3.up;
        }

        Rotate();
        Move();

        Shoot();
    }

    private void Rotate()
    {
        var currentCameraRotation = m_playerCamera.localRotation.eulerAngles;
        if (currentCameraRotation.x > 180f) currentCameraRotation.x -= 360f;

        var newRotation = currentCameraRotation.x + _lookInput.y * -m_lookUpAndDownSpeed * Time.deltaTime;
        currentCameraRotation.x = Mathf.Clamp(newRotation, -89, 89);
        m_playerCamera.localRotation = Quaternion.Euler(currentCameraRotation);

        var currentPlayerRotation = transform.rotation;
        currentPlayerRotation *= Quaternion.AngleAxis(_lookInput.x * m_rotationSpeed * Time.deltaTime, Vector3.up);
        transform.rotation = currentPlayerRotation;
    }

    private void Move()
    {
        var startPosition = transform.position;
        var clampedMoveInput = Vector2.ClampMagnitude(_movementInput, 1f);
        var unprojectMovement = clampedMoveInput * m_moveSpeed * Time.deltaTime;

        var movement = transform.forward * unprojectMovement.y;
        movement += transform.right * unprojectMovement.x;
        movement.y += _fallingSpeed * Time.deltaTime;

        if (_isGrounded)
        {
            movement = Vector3.ProjectOnPlane(movement, _groundNormal);
        }

        var capsuleWorldSpaceCenter = transform.TransformPoint(m_playerCollider.center);
        var capsuleBottom = capsuleWorldSpaceCenter - Vector3.up * m_playerCollider.height / 2f;
        var capsuleTop = capsuleWorldSpaceCenter + Vector3.up * m_playerCollider.height / 2f;

        /*
        if (Physics.CapsuleCast (capsuleBottom, capsuleTop, m_playerCollider.radius * 0.99f, movement.normalized, out var hit, movement.magnitude))
        {
            movement *= hit.distance / movement.magnitude;
            movement -= movement.normalized * m_playerCollider.radius * 1.01f;
        }

        */

        //position.x += movement.x;
        //position.z += movement.y;


        transform.position = startPosition + movement;
    }

    private void Shoot()
    {
        m_leftMouseButtonAttack.OnUpdate(m_daggerSpawnPoint, Input.GetMouseButton(0), ref _leftAttackCooldown);
        m_rightMouseButtonAttack.OnUpdate(m_daggerSpawnPoint, Input.GetMouseButton(1), ref _rightAttackCooldown);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}
