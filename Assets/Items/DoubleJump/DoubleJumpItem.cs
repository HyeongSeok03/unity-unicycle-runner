using UnityEngine;

public class DoubleJumpItem : Item
{
    protected override void ApplyEffect(Unicycle player)
    {
        player.doubleJumpActive = true;
        Destroy(gameObject);
    }
}
