using System.Collections;
using UnityEngine;

public class TiltItem : Item
{
    public override void ApplyEffect(Unicycle player)
    {
        base.ApplyEffect(player);
    }

    protected override IEnumerator EffectDurationCoroutine(Unicycle player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.freezeRotation = true;

        yield return new WaitForSeconds(effectDuration);

        rb.freezeRotation = false;
        Destroy(gameObject);
    }
}
