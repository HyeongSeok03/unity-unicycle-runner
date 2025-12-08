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

    public override void ApplyEffect (Unicycle player)
    {
        player.wing.SetActive(true);
        Rigidbody rb = player.GetComponent<Rigidbody>();
        defaultConstraints = rb.constraints;

        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero; // 혹시 모를 관성 제거
        rb.angularVelocity = Vector3.zero;

        // 이동 목표값 미리 계산
        float startY = player.transform.position.y;
        float targetY = startY + heightOffset;

        // --- DOTween 시퀀스 시작 ---
        Sequence flySequence = DOTween.Sequence();

        player.transform.DOMoveY(targetY, riseTime).SetEase(Ease.InOutQuad);
        player.transform.DORotate(Vector3.zero, riseTime).SetEase(Ease.InOutQuad);

        // 떠 있는 시간 대기
        flySequence.AppendInterval(floatTime);
        
        flySequence.OnComplete(() => RestorePlayer(player));
    }
    
    private void RestorePlayer(Unicycle player)
    {
        player.wing.SetActive(false);
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.constraints = defaultConstraints;
        rb.useGravity = true;
        player.isActiving = false;
    }
}