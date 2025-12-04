using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    
    public int level = 1;
    
    public List<Obstacle> obstacles = new List<Obstacle>();
    
    [Header("Obstacle Spawn")]
    public Vector3 spawnPosition = new Vector3(0f, 1f, 20f);
    public float spawnInterval = 5f;
    
    [Header("Obstacle Speed")]
    public float obstacleSpeed = 5f;
    public float speedIncreaseRate = 0.5f;
    public float maxObstacleSpeed = 20f;
    
    private float _initialSpeed;
    
    public static float GetObstacleSpeed()
    {
        return _instance.obstacleSpeed;
    }
    
    private void Awake()
    {
        _instance = this;
        _initialSpeed = obstacleSpeed;
        StartCoroutine(SpawnObstacleCoroutine());
    }
    
    private void Update()
    {
        // 시간이 지날수록 레벨 증가
        // 일정 시간마다 레벨을 올리는 로직
        
        // 시간이 지날수록 속도 증가
        if (obstacleSpeed < maxObstacleSpeed)
        {
            obstacleSpeed += speedIncreaseRate * Time.deltaTime;
            obstacleSpeed = Mathf.Min(obstacleSpeed, maxObstacleSpeed);
        }
    }
    
    private void SpawnObstacle()
    {
        var randomIndex = Random.Range(0, obstacles.Count);
        var obstaclePrefab = obstacles[randomIndex];
        
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
    
    private IEnumerator SpawnObstacleCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnObstacle();
            
            // 일정 시간마다 레벨업 (예: 10초마다)
            if (Time.time % 10f < spawnInterval)
            {
                LevelUp();
            }
        }
    }

    private void LevelUp()
    {
        level++;
        
        // 스폰 간격을 줄임 (최소 1초까지)
        spawnInterval = Mathf.Max(1f, spawnInterval - 0.2f);
        
        // 속도 증가율도 올림
        speedIncreaseRate += 0.1f;
        obstacleSpeed = _initialSpeed;
        
        print($"Level Up! 현재 레벨: {level}, 스폰 간격: {spawnInterval}초");
    }
    
}
