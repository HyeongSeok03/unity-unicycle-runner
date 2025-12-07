using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public float speed = 3f; // 이동 속도 (초당 단위)

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
}
