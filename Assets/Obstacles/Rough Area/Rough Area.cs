using UnityEngine;

public class RoughArea : MonoBehaviour
{
    [Header("Roughness Config")]
    public float torqueIntensity = 10f; // 좌우 흔들림 세기
    public float bounceIntensity = 2f;  // 위로 튀는 세기

    [Range(0f, 1f)]
    public float bumpFrequency = 0.2f;   // 덜컹거리는 빈도 (0.2 = 20% 확률로 프레임마다 튐)

    private void OnTriggerStay(Collider other)
    {
        // 최적화를 위해 태그 비교를 먼저 하는 것이 좋습니다 (선택사항)
        // if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<Unicycle>();
        if (player != null)
        {
            // 1. 확률 체크: 매 프레임 튀면 너무 정신없음
            if (Random.value < bumpFrequency)
            {
                // 당첨되면 툭! 침
                player.ApplyRoughness(torqueIntensity, bounceIntensity);
            }
        }
    }
}
