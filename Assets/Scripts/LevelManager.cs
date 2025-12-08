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
    public float moveSpeed = 5f;
    public float speedIncreaseRate = 0.1f;
    public float maxObstacleSpeed = 20f;
    
    public float initialSpeed;
    private float _levelTimer = 0f;
    
    public static float GetObstacleSpeed()
    {
        return instance.moveSpeed;
    }

    public static void SetObstacleSpeed(float speed)
    {
        instance.moveSpeed = speed;
    }
    
    private void Awake()
    {
        instance = this;
        initialSpeed = moveSpeed;
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
            
            if (moveSpeed < maxObstacleSpeed)
            {
                moveSpeed += speedIncreaseRate * 0.1f;
                moveSpeed = Mathf.Min(moveSpeed, maxObstacleSpeed);
            }
            score += level;
            OnScoreChanged?.Invoke(score);
        }
    }

    private void LevelUp()
    {
        level++;
        
        OnLevelChanged?.Invoke(level);
        
        // 속도 증가율도 올림
        speedIncreaseRate += 0.1f;
        
        print($"Level Up! 현재 레벨: {level}");
    }
}
