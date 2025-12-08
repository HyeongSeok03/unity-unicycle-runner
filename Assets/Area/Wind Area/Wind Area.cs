using UnityEngine;

public class WindArea : Area
{
    [SerializeField] private float torque = 5f;
    [SerializeField] private ParticleSystem windEffect;

    [SerializeField] private int _direction = 1;
    
    protected override void Apply(Unicycle player)
    {
        player.rb.AddTorque(new Vector3(0f, 0f, _direction * torque), ForceMode.Force);
    }
}
