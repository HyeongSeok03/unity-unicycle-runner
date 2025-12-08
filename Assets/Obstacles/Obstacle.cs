using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] public Vector3 offsetPosition;
    [SerializeField] protected Renderer obstacleRenderer;
    private void Start()
    {
        obstacleRenderer = GetComponent<Renderer>();
    }

    protected virtual void Hit(Unicycle player)
    {
        player.rb.constraints = RigidbodyConstraints.None;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            var player = other.GetComponent<Unicycle>();
            if (player.shieldActive)
            {
                player.shieldActive = false;
                Destroy(gameObject);
                return;
            }
            
            Hit(player);
            player.GameOver();
        }
    }
    
    
    private void Update()
    {
        //transform.Translate(Vector3.back * (LevelManager.GetObstacleSpeed() * Time.deltaTime));
        //if (transform.position.z < 0f)
        //{
        //    var newColor = obstacleRenderer.material.color;
        //        newColor.a = 0.5f;
                
        //    obstacleRenderer.material.color = newColor;
            
        //    Destroy(gameObject, 1f);
        //}
    }
}
