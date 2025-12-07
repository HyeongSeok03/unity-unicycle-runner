using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int score = 0;
    public event Action<int> OnScoreChanged;
    
    public int level = 1;
    public float timePerLevel = 10f;
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
        return instance.obstacleSpeed;
    }
    
    private void Awake()
    {
        instance = this;
        _initialSpeed = obstacleSpeed;
        StartCoroutine(SpawnObstacleCoroutine());
        StartCoroutine(IncreaseSpeedCoroutine());
    }
    
    private IEnumerator IncreaseSpeedCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            
            if (obstacleSpeed < maxObstacleSpeed)
            {
                obstacleSpeed += speedIncreaseRate * 0.1f;
                obstacleSpeed = Mathf.Min(obstacleSpeed, maxObstacleSpeed);
                score += level;
                OnScoreChanged?.Invoke(score);
            }
        }
    }
    
    private void SpawnObstacle()
    {
        var index = Random.Range(0, obstacles.Count);
        var obstacle = obstacles[index];
        var spawnPos = spawnPosition + obstacle.offsetPosition;
        
        Instantiate(obstacle, spawnPos, Quaternion.identity);
    }
    
    private IEnumerator SpawnObstacleCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnObstacle();
            
            if (Time.time % timePerLevel < spawnInterval)
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
