using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Unicycle : MonoBehaviour
{
    public static Unicycle instance;

    [Header("Physics")]
    public Rigidbody rb;
    public float tiltTorque = 50f;       // 회전 힘
    public float moveSpeed = 10f;        // 이동 속도
    public float jumpForce = 4f;         // 더블 점프에 사용할 상수 점프 힘

    [Range(-0.5f, 0.5f)] public float centerOfMassY = 0.1f; // 무게 중심 Y 위치
    [Range(0f, 1f)] public float moveTiltAngle = 0.1f;      // 이동 시 기울기 각도
    [Range(0f, 1f)] public float maxTiltAngle = 0.5f;       // 최대 기울기 각도

    [Header("Ground Check Settings")]
    public Vector3 groundCheckPosition;      // 지면 체크 위치
    public LayerMask groundLayer;            // 지면 레이어
    public float groundCheckRadius = 0.3f;   // 지면 체크 반경

    [Header("Item Active")]
    public GameObject wing;

    private int _moveDirection = 0;
    private bool _isGrounded;
    private Vector2 _moveInput;

    private Item _activeItem;
    public bool isActiving;
    
    public bool IsGrounded => _isGrounded;

    public bool shieldActive = false;

    [Header("Jump")]
    public InputActionReference jumpInput;
    public bool doubleJumpActive = false;

    // === 점프 게이지 관련 설정 ===
    [Header("Jump Charge Settings")]
    public float minJumpForce = 3f;    // 최소 점프 힘
    public float maxJumpForce = 12f;   // 최대 점프 힘
    public float chargeSpeed = 3f;     // 초당 게이지 증가 속도 (0~1 범위)

    private float _jumpCharge = 0f;        // 0~1 사이 게이지
    private bool _isChargingJump = false;  // 게이지 차는 중인지
    private bool _doubleJumpUsed = false;  // 더블 점프 사용 여부

    private string _jumpState = "Idle";    // Debug 용 상태 문자열

    public Slider jumpGaugeSlider;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip doubleJumpClip;
    [SerializeField] private AudioClip gameOverClip;
    
    public void SetActiveItem(Item item)
    {
        if (isActiving) return;
        _activeItem = item;
        Debug.Log("Set Item " + _activeItem.name);
    }
    
    private void Awake()
    {
        if (!instance)
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

        if (jumpGaugeSlider != null)
        {
            jumpGaugeSlider.minValue = 0f;
            jumpGaugeSlider.maxValue = 1f;
            jumpGaugeSlider.value = 0f;
        }
    }

    private void Update()
    {
        var action = jumpInput.action;

        // 점프 버튼 처음 눌렀을 때
        if (action.WasPressedThisFrame())
        {
            OnJumpPressed();
        }

        // 점프 버튼 누르고 있는 동안 (게이지 채우기)
        if (action.IsPressed())
        {
            OnJumpHeld();
        }

        // 점프 버튼을 뗐을 때
        if (action.WasReleasedThisFrame())
        {
            OnJumpReleased();
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        ApplyTilt();
        CheckRotation();
        ApplyMove();
    }

    private void UpdateJumpGaugeUI()
    {
        if (jumpGaugeSlider == null) return;
        jumpGaugeSlider.value = _jumpCharge;
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>(); // (x: A/D)
    }

    // =======================
    // 점프 입력 처리
    // =======================

    // 버튼이 처음 눌렸을 때
    private void OnJumpPressed()
    {
        if (_isGrounded)
        {
            // 바닥에 있을 때만 게이지 시작
            _isChargingJump = true;
            _jumpCharge = 0f;
            _jumpState = "Charging";
            UpdateJumpGaugeUI();
            Debug.Log($"[Jump] Start Charge | Grounded={_isGrounded}, Charge={_jumpCharge:F2}, State={_jumpState}");
        }
        else
        {
            // 공중 + 더블점프 가능하면 즉시 더블점프 (게이지 X)
            if (doubleJumpActive && !_doubleJumpUsed)
            {
                DoDoubleJump();
            }
        }
    }

    // 버튼을 누르고 있는 동안 (게이지 증가)
    private void OnJumpHeld()
    {
        if (_isChargingJump && _isGrounded)
        {
            _jumpCharge += chargeSpeed * Time.deltaTime;
            _jumpCharge = Mathf.Clamp01(_jumpCharge);

            _jumpState = "Charging";
            UpdateJumpGaugeUI();
            Debug.Log($"[Jump] Charging... | Charge={_jumpCharge:F2}, State={_jumpState}");
        }
    }

    // 버튼을 뗐을 때 (차지 점프 발동)
    private void OnJumpReleased()
    {
        if (_isChargingJump)
        {
            _isChargingJump = false;

            // 혹시 누르는 동안 떨어졌으면 점프 취소
            if (!_isGrounded)
            {
                _jumpState = "Cancel (No Ground)";
                Debug.Log($"[Jump] Charge Cancelled (No Ground) | Charge={_jumpCharge:F2}, State={_jumpState}");
                _jumpCharge = 0f;
                UpdateJumpGaugeUI();
                return;
            }

            PerformChargedJump();
        }
    }

    // 실제 차지 점프 수행
    private void PerformChargedJump()
    {
        // 게이지가 너무 낮으면 살짝 기본 보정
        float normalized = Mathf.Clamp01(_jumpCharge);
        float appliedForce = Mathf.Lerp(minJumpForce, maxJumpForce, normalized);

        var vel = rb.linearVelocity;
        vel.y = appliedForce;
        rb.linearVelocity = vel;

        if (audioSource && jumpClip)
            audioSource.PlayOneShot(jumpClip);

        _jumpState = "Jump";
        Debug.Log($"[Jump] Charged Jump! | Charge={_jumpCharge:F2}, Force={appliedForce:F2}, State={_jumpState}");

        _jumpCharge = 0f;
        UpdateJumpGaugeUI();
    }

    // 더블점프 (게이지 없이 상수 점프)
    private void DoDoubleJump()
    {
        _doubleJumpUsed = true;

        var vel = rb.linearVelocity;
        vel.y = jumpForce; // 상수 점프 힘
        rb.linearVelocity = vel;

        if (audioSource && doubleJumpClip)
            audioSource.PlayOneShot(doubleJumpClip);

        _jumpState = "DoubleJump";
        Debug.Log($"[Jump] Double Jump! | Force={jumpForce:F2}, State={_jumpState}");
    }

    private void OnAttack(InputValue value)
    {
        if (!_activeItem) return;
        
        _activeItem?.ApplyEffect(this);
        isActiving = true;
        Debug.Log("Interact with item: " + _activeItem?.name);
        _activeItem = null;
    }

    // =======================
    // 지면 체크
    // =======================
    private bool _prevGrounded = false;

    private void CheckGrounded()
    {
        var position = transform.position + groundCheckPosition;
        _isGrounded = Physics.CheckSphere(position, groundCheckRadius, groundLayer);

        if (_isGrounded)
        {
            // 땅 밟으면 더블 점프 가능 상태로 초기화
            if (!_prevGrounded)
            {
                Debug.Log("[Ground] Landed. Reset double jump.");
            }
            _doubleJumpUsed = false;
        }

        // 상태 변화 로그
        if (_prevGrounded != _isGrounded)
        {
            Debug.Log($"[Ground] GroundState Changed: {_prevGrounded} -> {_isGrounded}");
            _prevGrounded = _isGrounded;
        }
    }

    // =======================
    // 기울기 / 이동
    // =======================
    private void ApplyTilt()
    {
        var torque = new Vector3(0f, 0f, -_moveInput.x);
        var worldTorque = transform.TransformDirection(torque);

        rb.AddTorque(worldTorque * tiltTorque, ForceMode.Acceleration);
    }

    private void CheckRotation()
    {
        var rotation = transform.rotation.z;

        if (Math.Abs(rotation) < moveTiltAngle)
        {
            _moveDirection = 0;
        }
        else
        {
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
        tilt = Mathf.Pow(tilt, 2); // 제곱하여 민감도 조절 가능
        return moveSpeed * tilt;
    }

    // =======================
    // 게임오버
    // =======================
    private const float GameOverTorque = 3f;
    private const float GameOverForce = 10f;

    public void GameOver()
    {
        if (audioSource && gameOverClip)
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
        // 1. 기존 GroundCheck 시각화
        Gizmos.color = Color.cyan;
        var position = transform.position + groundCheckPosition;
        Gizmos.DrawWireSphere(position, groundCheckRadius);

        Gizmos.color = Color.red;

        Vector3 localCOM = new Vector3(0, centerOfMassY, 0);
        Vector3 drawPos = transform.TransformPoint(localCOM);
        Gizmos.DrawSphere(drawPos, 0.1f);
    }
}
