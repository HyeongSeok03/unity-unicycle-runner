using UnityEngine;

public class WindArea : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private bool rightWind = true;
    [SerializeField] private float torque = 5f;

    void OnTriggerEnter(Collider other)
    {
        Unicycle player = other.GetComponent<Unicycle>();
        if (player != null)
        {

        }
    }

    void OnTriggerStay(Collider other)
    {
        Unicycle player = other.GetComponent<Unicycle>();
        if (player != null)
        {
            player.ApplyTiltExternalTorque(rightWind, torque);
        }
    }
}
