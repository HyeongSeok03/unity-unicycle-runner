using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("Obstacle Spawn")]
    public List<Obstacle> obstacles = new List<Obstacle>();
    public Vector3 spawnPosition = new Vector3(0f, 1f, 20f);
    public float spawnInterval = 5f;
    public float minSpawnInterval = 1f;

    private void Start()
    {
        LevelManager.instance.OnLevelChanged += OnLevelChanged;
        StartCoroutine(SpawnObstacleCoroutine());
    }

    private void OnLevelChanged(int level)
    {
        spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - 0.2f);
        
        print($"스폰 간격: {spawnInterval}초");
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
        }
    }
}

