using UnityEngine;

public class IceArea : MonoBehaviour
{
    [SerializeField] private float iceTiltTorqueMultiplier = 1.3f; // 얼음 지역에서의 기울기 토크 배율

    void OnTriggerEnter(Collider other)
    {
        var unicycle = other.GetComponent<Unicycle>();
        if (unicycle)
        {
            unicycle.SetOnIce(true, iceTiltTorqueMultiplier);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var unicycle = other.GetComponent<Unicycle>();
        if (unicycle)
        {
            unicycle.SetOnIce(false, iceTiltTorqueMultiplier);
        }
    }
}
