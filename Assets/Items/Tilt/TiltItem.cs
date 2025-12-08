using System.Collections;
using UnityEngine;

public class TiltItem : Item
{
    private Rigidbody rb;
    RigidbodyConstraints defaultConstraints;
    
    public override void ApplyEffect(Unicycle player)
    {
        rb = player.GetComponent<Rigidbody>();
        base.ApplyEffect(player);
    }

    protected override IEnumerator EffectDurationCoroutine(Unicycle player)
    {
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        defaultConstraints = rb.constraints;
        rb.freezeRotation = true;

        yield return new WaitForSeconds(effectDuration);

        rb.constraints = defaultConstraints;
        player.isActiving = false;
        Destroy(gameObject);
    }
}
