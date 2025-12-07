using UnityEngine;

public class WindArea : Area
{
    [SerializeField] private float torque = 5f;
    [SerializeField] private ParticleSystem windEffect;
    
    private int _direction = 1;
    
    private void Start()
    {
        _direction = -1;
        windEffect.transform.localScale = new Vector3(1f, 1f, -1 * _direction);
    }
    
    protected override void Apply(Unicycle player)
    {
        player.rb.AddTorque(new Vector3(0f, 0f, _direction * torque), ForceMode.Force);
    }
}
