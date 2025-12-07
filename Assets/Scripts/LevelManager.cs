using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int score = 0;
    public event Action<int> OnScoreChanged;
    
    public int level = 1;
    public event Action<int> OnLevelChanged;
    
    public float timePerLevel = 10f;
    
    [Header("Obstacle Speed")]
    public float obstacleSpeed = 5f;
    public float speedIncreaseRate = 0.5f;
    public float maxObstacleSpeed = 20f;
    
    private float _initialSpeed;
    private float _levelTimer = 0f;
    private ObstacleManager _manager;
    
    public static float GetObstacleSpeed()
    {
        return instance.obstacleSpeed;
    }
    
    private void Awake()
    {
        instance = this;
        _initialSpeed = obstacleSpeed;
        _manager = GetComponent<ObstacleManager>();
        StartCoroutine(IncreaseSpeedCoroutine());
    }
    
    private void Update()
    {
        _levelTimer += Time.deltaTime;
        if (_levelTimer >= timePerLevel)
        {
            LevelUp();
            _levelTimer = 0f;
        }
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

    private void LevelUp()
    {
        level++;
        
        OnLevelChanged?.Invoke(level);
        
        // 속도 증가율도 올림
        speedIncreaseRate += 0.1f;
        obstacleSpeed = _initialSpeed;
        
        print($"Level Up! 현재 레벨: {level}");
    }
}
