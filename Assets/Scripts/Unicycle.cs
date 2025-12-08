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
    [Range(0f, 1f)] public float gameOverTiltAngle = 0.7f; // 게임 오버 기울기 각도

    [Header("Ground Check Settings")]
    public Vector3 groundCheckPosition; // 지면 체크 위치
    public LayerMask groundLayer;      // 지면 레이어
    public float groundCheckRadius = 0.3f; // 지면 체크 반경

    [Header("Item Active")]
    public GameObject wing;
    public GameObject shield;
    
    private int _moveDirection = 0;
    private bool _isGrounded;
    private Vector2 _moveInput;
    
    private Item _activeItem;
    
    public bool doubleJumpActive = false;
    public bool IsGrounded => _isGrounded;
    
    public bool shieldActive = false;
    
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

    private void OnJump(InputValue value)
    {
        if(_isGrounded || CanDoubleJump())
            rb.linearVelocity = new Vector3(0, jumpForce);
    }

    private void OnAttack(InputValue value)
    {
        if (!_activeItem) return;
        _activeItem?.ApplyEffect(this);
        Debug.Log("Interact with item: " + _activeItem?.name);
        _activeItem = null;
    }
    
    private bool CanDoubleJump()
    {
        if(!_isGrounded && doubleJumpActive)
        {
            doubleJumpActive = false;
            return true;
        }
        return false;
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
            GameManager.instance.GameOver();
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