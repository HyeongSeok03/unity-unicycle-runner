using System.Collections;
using UnityEngine;

public class TimeSlowItem : Item
{
    public override void ApplyEffect(Unicycle player)
    {
        base.ApplyEffect(player);
    }

    protected override IEnumerator EffectDurationCoroutine(Unicycle player)
    {
        Time.timeScale = 0.5f; // 시간 느리게
        yield return new WaitForSecondsRealtime(effectDuration);
        Time.timeScale = 1f; // 시간 정상화
        Destroy(gameObject);
    }
}
