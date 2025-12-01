using UnityEngine;
using UnityEngine.InputSystem;

public class Unicycle : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody rb;
    public float tiltTorque = 50f;       // 회전 힘
    public float autoBalanceForce = 5f;  // 중심 복원 세기
    public float maxTiltAngle = 30f;     // 최대 기울기 제한

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
}
