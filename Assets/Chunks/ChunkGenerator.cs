using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Chunk Pool (Prefab들)")]
    public GameObject startChunk;          // 첫 청크 고정 프리팹
    public GameObject[] chunkPrefabs;      // 랜덤으로 뽑을 청크 프리팹들

    [Header("Chunk Settings")]
    public int chunkCount = 5;             // 유지할 청크 개수
    public float chunkLength = 60f;        // 한 청크 길이 (z 방향)
    public float startOffset = 25f;        // 시작할때 플레이어의 얼마나 앞에 있을지.

    [Header("Item Settings")]
    public GameObject[] itemPrefabs;       // 아이템 프리팹들
    public float itemSpawnProbability = 0.2f; // 아이템 스폰 확률
    
    [Header("Move Settings")]
    public float moveSpeed => LevelManager.instance.moveSpeed;          // 뒤로 움직이는 속도

    [Header("Destroy Settings")]
    public Transform player;               // 플레이어 기준점
    public float destroyDistanceBehindPlayer = 80f;

    private readonly List<GameObject> activeChunks = new List<GameObject>();

    private void Start()
    {
        float startZ = startOffset;

        // 1️⃣ 맨 처음 청크는 startChunk로 고정 생성
        if (startChunk != null)
        {
            GameObject first = Instantiate(startChunk, new Vector3(0f, 0f, startZ), startChunk.transform.rotation, transform);
            activeChunks.Add(first);
        }
        else
        {
            Debug.LogWarning("Start Chunk가 지정되지 않았습니다!");
        }

        // 2️⃣ 나머지 청크들은 랜덤으로 생성
        for (int i = 1; i < chunkCount; i++)
        {
            float spawnZ = startZ + i * chunkLength;
            SpawnChunk(spawnZ);
        }
    }

    private void Update()
    {
        MoveChunks();
        RecycleChunksIfNeeded();
    }

    private void MoveChunks()
    {
        if (moveSpeed <= 0f) return;

        Vector3 move = Vector3.back * moveSpeed * Time.deltaTime;

        foreach (GameObject chunk in activeChunks)
        {
            chunk.transform.Translate(move, Space.World);
        }
    }

    private void RecycleChunksIfNeeded()
    {
        if (activeChunks.Count == 0) return;

        GameObject firstChunk = activeChunks[0];
        float playerZ = player != null ? player.position.z : 0f;

        if (firstChunk.transform.position.z < playerZ - destroyDistanceBehindPlayer)
        {
            activeChunks.RemoveAt(0);
            Destroy(firstChunk);

            GameObject lastChunk = activeChunks[activeChunks.Count - 1];
            float newZ = lastChunk.transform.position.z + chunkLength;
            SpawnChunk(newZ);
        }
    }

    private void SpawnChunk(float zPos)
    {
        if (chunkPrefabs == null || chunkPrefabs.Length == 0)
        {
            Debug.LogWarning("chunkPrefabs가 비어 있음");
            return;
        }

        int randIndex = Random.Range(0, chunkPrefabs.Length);
        GameObject prefab = chunkPrefabs[randIndex];

        Vector3 spawnPos = new Vector3(0f, 0f, zPos);
        Quaternion spawnRot = prefab.transform.rotation;

        GameObject chunk = Instantiate(prefab, spawnPos, spawnRot, transform);
        activeChunks.Add(chunk);
        
        var isSpawnItem = Random.value < itemSpawnProbability;
        if (isSpawnItem)
        {
            var itemIndex = Random.Range(0, itemPrefabs.Length);
            var itemPrefab = itemPrefabs[itemIndex];
            var randX = Random.Range(-15f, 15f);
            var itemSpawnPos = new Vector3(randX, -2f, zPos);
            Instantiate(itemPrefab, itemSpawnPos, itemPrefab.transform.rotation, chunk.transform);
        }
    }
}
