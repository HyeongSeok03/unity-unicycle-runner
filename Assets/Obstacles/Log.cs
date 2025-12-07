using UnityEngine;

public class Log : Obstacle
{
    [SerializeField] private float rotationTorque = 3f;
    [SerializeField] private float springForce = 10f;
    
    protected override void Hit(Unicycle player)
    {
        base.Hit(player);



        var dz = Random.Range(-rotationTorque, rotationTorque);
        var torque = new Vector3(rotationTorque, 0f, dz);
        var worldTorque = transform.TransformDirection(torque);
        
        player.rb.AddTorque(worldTorque, ForceMode.Impulse);
        player.rb.linearVelocity = new Vector3(0f, springForce, 0f);
    }
}
