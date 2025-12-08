using UnityEngine;

public class ShieldItem : Item
{
    protected override void ApplyEffect(Unicycle player)
    {
        player.shieldActive = true;
        player.shield.SetActive(true);
        
        Destroy(gameObject);
    }
}
