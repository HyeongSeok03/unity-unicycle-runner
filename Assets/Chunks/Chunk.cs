using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3 size = new Vector3(10f, 10f, 10f); // 10 * 10 * 10
    public Color color = Color.cyan;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
