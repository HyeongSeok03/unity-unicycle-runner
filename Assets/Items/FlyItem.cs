using System.Collections;
using UnityEngine;

public class FlyItem : Item
{
    [SerializeField] private float heightOffset = 2f;
    [SerializeField] private float riseTime = 0.5f;
    [SerializeField] private float floatTime = 2f; // 떠 있는 시간
    [SerializeField] private float fallTime = 0.3f; // 내려가는 시간 (빠르게)
    RigidbodyConstraints defaultConstraints;

    protected override void ApplyEffect(Unicycle player)
    {
        base.ApplyEffect(player);
    }

    protected override IEnumerator EffectDurationCoroutine(Unicycle player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        defaultConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;

        // 부드럽게 올라가기
        yield return StartCoroutine(SmoothRise(player.transform));

        // 떠 있는 시간 대기
        yield return new WaitForSeconds(floatTime);

        // 빠르게 내려가기
        yield return StartCoroutine(SmoothFall(player.transform));

        // 내려간 후에 freeze 해제
        rb.constraints = defaultConstraints;
        rb.useGravity = true;
        Destroy(gameObject);
    }

    private IEnumerator SmoothRise(Transform player)
    {
        Vector3 startPos = player.position;
        Vector3 targetPos = startPos + Vector3.up * heightOffset;
        Quaternion startRot = player.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);

        float elapsed = 0f;

        while (elapsed < riseTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / riseTime);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            
            player.position = Vector3.Lerp(startPos, targetPos, smoothT);
            player.rotation = Quaternion.Slerp(startRot, targetRot, smoothT);
            
            yield return null;
        }

        player.position = targetPos;
        player.rotation = targetRot;
    }

    private IEnumerator SmoothFall(Transform player)
    {
        Vector3 startPos = player.position;
        Vector3 targetPos = startPos - Vector3.up * heightOffset;

        float elapsed = 0f;

        while (elapsed < fallTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fallTime);
            // EaseIn 효과로 점점 빠르게 떨어지는 느낌
            float easeT = t * t;
            
            player.position = Vector3.Lerp(startPos, targetPos, easeT);
            
            yield return null;
        }

        player.position = targetPos;
    }
}
