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
    public float moveRange = 5f;         // 이동 범위
    public float jumpForce = 8f;        // 점프 힘
    public float centerOfMassY = 0.1f; // 무게 중심 높이

    [Range(-0.5f, 0.5f)] public float centerOfMassY = 0.1f; // 무게 중심 Y 위치
    [Range(0f, 1f)] public float moveTiltAngle = 0.1f;   // 이동 시 기울기 각도
    [Range(0f, 1f)] public float maxTiltAngle = 0.5f;    // 최대 기울기 각도
    [Range(0f, 1f)] public float gameOverTiltAngle = 0.7f; // 게임 오버 기울기 각도

    [Header("Ground Check Settings")]
    public Vector3 groundCheckPosition; // 지면 체크 위치
    public LayerMask groundLayer;      // 지면 레이어
    public float groundCheckRadius = 0.3f; // 지면 체크 반경
    
    private int _moveDirection = 0;
    private bool _isGrounded;
    private Vector2 _moveInput;
    
    public bool isGrounded => _isGrounded;
    
    
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        var com = rb.centerOfMass;
        com.y = centerOfMassY;
        rb.centerOfMass = com;
    }
    
    private void FixedUpdate()
    {
        CheckGrounded();
        ApplyTilt();
        CheckRotation();
        ApplyMove();
    }

    private void OnDisable()
    {
        StageManager.GameOver();
    }
    
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>(); // (x: A/D)
    }

    private void OnJump(InputValue value)
    {
        if (!_isGrounded) return;
        rb.linearVelocity = new Vector3(0, jumpForce);
    }
    
    private void CheckGrounded()
    {
        var position = transform.position + groundCheckPosition;
        _isGrounded = Physics.CheckSphere(position, groundCheckRadius, groundLayer);
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

        if (Mathf.Abs(rotation) > gameOverTiltAngle)
        {
            Destroy(gameObject, 1f);
        }
    }

    private void ApplyMove()
    {
        if (_moveDirection != 0)
        {
            var moveSpeedWithTilt = GetMoveSpeed();
            var moveOffset = transform.right * (_moveDirection * moveSpeedWithTilt * Time.fixedDeltaTime);

            var targetPosition = rb.position + moveOffset;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -moveRange, moveRange);

            rb.MovePosition(targetPosition);
        }
    }

    private float GetMoveSpeed()
    {
        var tilt = Mathf.Min(Mathf.Abs(transform.rotation.z), maxTiltAngle) / maxTiltAngle;
            tilt = Mathf.Pow(tilt, 2); // 제곱하여 민감도 조절
        return moveSpeed * tilt;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        var position = transform.position + groundCheckPosition;
        Gizmos.DrawWireSphere(transform.position + groundCheckPosition, groundCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rb.centerOfMass + centerOfMassHeight * Vector3.up, 0.1f);
    }
}
