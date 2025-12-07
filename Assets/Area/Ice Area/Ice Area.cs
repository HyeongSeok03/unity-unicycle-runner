using UnityEngine;

public class IceArea : Area
{
    [SerializeField] private float iceTorqueMultiplier = 1.5f; // 얼음 지역에서의 기울기 토크 배율
    [SerializeField] private float iceSpeedMultiplier = 1.2f; // 얼음 지역에서의 속도 배율

    private float _originalTorque;
    private float _originalSpeed;
    
    protected override void Enter(Unicycle player)
    {
        _originalSpeed = player.moveSpeed;
        _originalTorque = player.tiltTorque;
        
        player.moveSpeed *= iceSpeedMultiplier;
        player.tiltTorque *= iceTorqueMultiplier;
    }

    protected override void Exit(Unicycle player)
    {
        player.moveSpeed = _originalSpeed;
        player.tiltTorque = _originalTorque;
    }
}
