using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public Vector3 offsetPosition;
    [SerializeField] protected Renderer obstacleRenderer;
    
    protected virtual void Hit(Unicycle player)
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Unicycle>();
            Hit(player);
            Destroy(player.gameObject, 2f);
        }
    }
    
    private void Start()
    {
        moveSpeed = LevelManager.GetObstacleSpeed();
    }
    
    private void Update()
    {
        transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime));
        if (transform.position.z < 0f)
        {
            var newColor = obstacleRenderer.material.color;
                newColor.a = 0.5f;
                
            obstacleRenderer.material.color = newColor;
            
            Destroy(gameObject, 1f);
        }
    }
}
