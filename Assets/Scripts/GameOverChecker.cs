using UnityEngine;

public class GameOverChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged"))
        {
            Debug.Log("Game Over Trigger hit by non-player object: " + other.name);
            Unicycle.instance.GameOver();
        }
    }
}
