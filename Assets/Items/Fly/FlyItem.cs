using System.Collections;
using DG.Tweening; // DOTween 필수
using UnityEngine;

public class FlyItem : Item
{
    [SerializeField] private float heightOffset = 2f;
    [SerializeField] private float riseTime = 0.5f;
    [SerializeField] private float floatTime = 2f;
    [SerializeField] private float fallTime = 0.3f;

    RigidbodyConstraints defaultConstraints;

    protected override IEnumerator EffectDurationCoroutine(Unicycle player)
    {
        player.wing.SetActive(true);
        Rigidbody rb = player.GetComponent<Rigidbody>();
        defaultConstraints = rb.constraints;

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero; // 혹시 모를 관성 제거

        // 이동 목표값 미리 계산
        float startY = player.transform.position.y;
        float targetY = startY + heightOffset;

        // --- DOTween 시퀀스 시작 ---
        Sequence flySequence = DOTween.Sequence();

        flySequence.Append(player.transform.DOMoveY(targetY, riseTime).SetEase(Ease.InOutQuad));
        flySequence.Join(player.transform.DORotate(Vector3.zero, riseTime).SetEase(Ease.InOutQuad));

        // 떠 있는 시간 대기
        flySequence.AppendInterval(floatTime);

        // 천천히 내려오기
        flySequence.Append(player.transform.DOMoveY(startY, fallTime).SetEase(Ease.InQuad));

        // 시퀀스가 모두 끝날 때까지 대기
        yield return flySequence.WaitForCompletion();

        // --- 종료 및 복구 ---
        rb.constraints = defaultConstraints;
        rb.useGravity = true;
        player.wing.SetActive(false);
        Destroy(gameObject);
    }
}