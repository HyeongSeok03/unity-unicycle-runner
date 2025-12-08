using UnityEngine;

public class DoubleJumpItem : Item
{
    public override void ApplyEffect(Unicycle player)
    {
        player.doubleJumpActive = true;
        Destroy(gameObject);
    }
}
