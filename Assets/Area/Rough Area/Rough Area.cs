using UnityEngine;

public class RoughArea : Area
{
    [Header("Roughness Config")]
    public float torqueForce = 10f; // 좌우 흔들림 세기
    public float springForce = 2f;  // 위로 튀는 세기

    protected override void Apply(Unicycle player)
    {
        
        var randomTorque = Random.Range(-torqueForce, torqueForce);
        player.rb.AddTorque(new Vector3(0f, 0f, randomTorque), ForceMode.Force);

        if (player.isGrounded)
        {
            var randomSpring = Random.Range(0f, springForce);
            player.rb.AddForce(Vector3.up * randomSpring, ForceMode.Impulse);
        }
            
    }
}
