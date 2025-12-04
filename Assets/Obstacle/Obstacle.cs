using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    protected virtual void Hit(Unicycle player)
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Unicycle>();
            Hit(player);
        }
    }
    
    private void Start()
    {
        moveSpeed = LevelManager.GetObstacleSpeed();
    }
    
    private void Update()
    {
        transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime));
    }
}
