using UnityEngine;

public class Wall : Obstacle
{
    [SerializeField] private float rotationTorque = 3f;
    [SerializeField] private float springForce = 10f;

    private void Awake()
    {
        var possibleX = new float[]{ -3f, 0f, 3f };
        var idx = Random.Range(0, possibleX.Length);
        var pos = transform.position;
        
        pos.x = possibleX[idx];
        // transform.position = pos;
    }
    
    protected override void Hit(Unicycle player)
    {

        var dz = Random.Range(-rotationTorque, rotationTorque);
        var torque = new Vector3(rotationTorque, 0f, dz);
        var worldTorque = transform.TransformDirection(torque);
        
        player.rb.AddTorque(worldTorque, ForceMode.Impulse);
        
        var knockbackForce = new Vector3(0f, springForce, -springForce * 2f);
        player.rb.linearVelocity = knockbackForce;
    }
}
