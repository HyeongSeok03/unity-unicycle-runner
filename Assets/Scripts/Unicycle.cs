using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Unicycle : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody rb;
    public float tiltTorque = 50f;       // 회전 힘
    public float moveSpeed = 10f;        // 이동 속도

    [Range(0f, 1f)]
    public float moveTiltAngle = 0.1f;   // 이동 시 기울기 각도

    [Range(0f, 1f)]
    public float maxTiltAngle = 0.5f;    // 최대 기울기 각도

    [Range(0f, 1f)]
    public float gameOverTiltAngle = 0.7f; // 게임 오버 기울기 각도

    private int moveDirection = 0;

    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Vector3 com = rb.centerOfMass;
        com.y = -0.5f;
        rb.centerOfMass = com;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>(); // (x: A/D, y: W/S)
    }

    private void FixedUpdate()
    {
        ApplyTilt();

        CheckRotation();
        ApplyMove();
    }

    private void ApplyTilt()
    {
        // 입력값으로 토크 생성
        Vector3 torque = new Vector3(
            moveInput.y, // W/S = pitch 방향 (앞/뒤 기울임)
             0f,
            -moveInput.x   // A/D = roll 방향 (좌/우 기울임)
        );

        Vector3 worldTorque = transform.TransformDirection( torque );

        rb.AddTorque(worldTorque * tiltTorque, ForceMode.Acceleration);
    }

    private void CheckRotation()
    {
        var rotation = transform.rotation.z;

        if (Math.Abs(rotation) < moveTiltAngle) {
            moveDirection = 0;
        }
        else {
            moveDirection = rotation > 0 ? -1 : 1;
        }

        if (Mathf.Abs(rotation) > gameOverTiltAngle)
        {
            Debug.Log("Game Over!");
        }
    }

    private void ApplyMove()
    {
        if (moveDirection != 0)
        {
            var moveSpeedWithTilt = GetMoveSpeed();
            var moveOffset = transform.right * moveDirection * moveSpeedWithTilt * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + moveOffset);
        }
    }

    private float GetMoveSpeed()
    {
        var tilt = Mathf.Min(Mathf.Abs(transform.rotation.z), maxTiltAngle) / maxTiltAngle;
            tilt = Mathf.Pow(tilt, 2); // 제곱하여 민감도 조절
        return moveSpeed * tilt;
    }
}
