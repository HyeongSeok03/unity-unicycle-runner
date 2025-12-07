using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float speed = 3f; // 이동 속도 (초당 단위)

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    public Vector3 size = new Vector3(10f, 10f, 10f); // 10 * 10 * 10
    public Color color = Color.cyan;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
