using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Unicycle : MonoBehaviour
{
    public static Unicycle instance;
    
    [Header("Physics")]
    public Rigidbody rb;
    public float tiltTorque = 50f;       // 회전 힘
    public float moveSpeed = 10f;        // 이동 속도
    public float jumpForce = 8f;        // 점프 힘

    [Range(-0.5f, 0.5f)] public float centerOfMassY = 0.1f; // 무게 중심 Y 위치
    [Range(0f, 1f)] public float moveTiltAngle = 0.1f;   // 이동 시 기울기 각도
    [Range(0f, 1f)] public float maxTiltAngle = 0.5f;    // 최대 기울기 각도

    [Header("Ground Check Settings")]
    public Vector3 groundCheckPosition; // 지면 체크 위치
    public LayerMask groundLayer;      // 지면 레이어
    public float groundCheckRadius = 0.3f; // 지면 체크 반경

    [Header("Item Active")]
    public GameObject wing;

    
    
    private int _moveDirection = 0;
    private bool _isGrounded;
    private Vector2 _moveInput;
    
    private Item _activeItem;
    
    public bool IsGrounded => _isGrounded;
    
    public bool shieldActive = false;
    
    [Header("Jump")]
    public InputActionReference jumpInput;
    public bool doubleJumpActive = false;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip doubleJumpClip;
    [SerializeField] private AudioClip gameOverClip;
    private bool _doubleJumpUsed = false;
    
    public void SetActiveItem(Item item)
    {
        _activeItem = item;
        Debug.Log("Set Item " + _activeItem.name);
    }
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        var com = rb.centerOfMass;
        com.y = centerOfMassY;
        rb.centerOfMass = com;
    }

    private void Update()
    {
        if (jumpInput.action.WasPressedThisFrame())
        {
            Jump();
        }
        else if (jumpInput.action.WasReleasedThisFrame())
        {
            JumpCanceled();
        }
    }
    
    private void FixedUpdate()
    {
        CheckGrounded();
        ApplyTilt();
        CheckRotation();
        ApplyMove();
    }
    
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>(); // (x: A/D)
    }

    private void Jump()
    {
        if(CanJump())
            rb.linearVelocity = new Vector3(0, jumpForce);
    }
    
    private void JumpCanceled()
    {
        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y * 0.5f);
        }
    }

    private void OnAttack(InputValue value)
    {
        if (!_activeItem) return;
        _activeItem?.ApplyEffect(this);
        Debug.Log("Interact with item: " + _activeItem?.name);
        _activeItem = null;
    }
    
    private bool CanJump()
    {
        if (_isGrounded)
        {
            audioSource.PlayOneShot(jumpClip);
            return true;
        }
        if (doubleJumpActive && !_doubleJumpUsed)
        {
            _doubleJumpUsed = true;
            audioSource.PlayOneShot(doubleJumpClip);
            return true;
        }
        return false;
    }


    private void CheckGrounded()
    {
        var position = transform.position + groundCheckPosition;
        _isGrounded = Physics.CheckSphere(position, groundCheckRadius, groundLayer);
        
        if (_isGrounded)
            _doubleJumpUsed = false;
    }

    private void ApplyTilt()
    {
        var torque = new Vector3(0f, 0f, -_moveInput.x);
        var worldTorque = transform.TransformDirection(torque);
        
        rb.AddTorque(worldTorque * tiltTorque, ForceMode.Acceleration);
    }
    
    private void CheckRotation()
    {
        var rotation = transform.rotation.z;

        if (Math.Abs(rotation) < moveTiltAngle) {
            _moveDirection = 0;
        }
        else {
            _moveDirection = rotation > 0 ? -1 : 1;
        }
    }

    private void ApplyMove()
    {
        if (_moveDirection != 0)
        {
            var moveSpeedWithTilt = GetMoveSpeed();
            var moveOffset = transform.right * (_moveDirection * moveSpeedWithTilt * Time.fixedDeltaTime);

            var targetPosition = rb.position + moveOffset;

            rb.MovePosition(targetPosition);
        }
    }

    private float GetMoveSpeed()
    {
        var tilt = Mathf.Min(Mathf.Abs(transform.rotation.z), maxTiltAngle) / maxTiltAngle;
            // tilt = Mathf.Pow(tilt, 2); // 제곱하여 민감도 조절
        return moveSpeed * tilt;
    }

    private const float GameOverTorque = 3f;
    private const float GameOverForce = 10f;
    public void GameOver()
    {
        audioSource.PlayOneShot(gameOverClip);
        
        rb.constraints = RigidbodyConstraints.None;
        var dz = UnityEngine.Random.Range(-GameOverTorque, GameOverTorque);
        var torque = new Vector3(GameOverTorque, 0f, dz);
        var worldTorque = transform.TransformDirection(torque);
        
        rb.AddTorque(worldTorque, ForceMode.Impulse);
        
        var knockbackForce = new Vector3(0f, GameOverForce, -GameOverForce * 0.5f);
        rb.linearVelocity = knockbackForce;
        
        GameManager.instance.GameOver();
    }

    private void OnDrawGizmosSelected()
    {
        // 1. 기존 GroundCheck 시각화 (그대로 유지)
        Gizmos.color = Color.cyan;
        var position = transform.position + groundCheckPosition;
        Gizmos.DrawWireSphere(position, groundCheckRadius);

        Gizmos.color = Color.red;

        Vector3 localCOM = new Vector3(0, centerOfMassY, 0);
        Vector3 drawPos = transform.TransformPoint(localCOM);
        Gizmos.DrawSphere(drawPos, 0.1f);
    }
}