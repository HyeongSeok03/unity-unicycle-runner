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
    public float moveTiltAngle = 0.3f;   // 이동 시 기울기 각도

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

    private void ApplyMove()
    {
        moveDirection = 0;
        
        if (Math.Abs(transform.rotation.z) > moveTiltAngle)
        {
            moveDirection = transform.rotation.z > 0 ? -1 : 1; 
        }

        if (moveDirection != 0)
        {
            Vector3 moveOffset = transform.right * moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveOffset);
        }
    }
}
