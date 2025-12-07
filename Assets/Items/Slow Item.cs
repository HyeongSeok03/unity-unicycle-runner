using System.Collections;
using UnityEngine;

public class SlowItem : Item
{
    [SerializeField] private float slowModifier = 0.5f;

    protected override void ApplyEffect(Unicycle player)
    {
        base.ApplyEffect(player);
    }

    protected override IEnumerator EffectDurationCoroutine(Unicycle player)
    {
        float originalSpeed = LevelManager.GetObstacleSpeed();

        LevelManager.SetObstacleSpeed(originalSpeed * 0.5f); // 속도 절반으로 감소

        yield return new WaitForSeconds(effectDuration);

        LevelManager.SetObstacleSpeed(originalSpeed); // 원래 속도로 복원
        Destroy(gameObject);
    }
}
