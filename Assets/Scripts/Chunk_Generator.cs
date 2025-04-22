using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour
{
    public List<GameObject> chunkPrefabs = new List<GameObject>();
    public Transform player;
    public GameObject initialChunk;
    public GameObject endChunk;

    public float chunkHeight = 5f;
    public float borderThreshold = 2f;
    public int maxChunks = 5;
    public float spawnDistance = 10f;
    public int chunksAhead = 1;
    public int BiomeLength = 3;
    public int maxBiomeLength = 5;

    private Queue<GameObject> activeChunks = new Queue<GameObject>();
    private GameObject currentBiomePrefab;
    private int currentBiomeCount = 0;
    private bool spawnEndChunkNext = false;
    private bool endChunkSpawned = false;

    void Start()
    {
        if (chunkPrefabs == null || chunkPrefabs.Count == 0) return;
       if (initialChunk == null) return;

        GameObject firstChunk = Instantiate(initialChunk, Vector3.zero, initialChunk.transform.rotation);
        activeChunks.Enqueue(firstChunk);
        currentBiomePrefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        currentBiomeCount = 1;
    }

    void Update()
    {
        if (endChunkSpawned) return;

        GameObject currentChunk = GetCurrentChunk();
        if (currentChunk == null) return;

        float chunkCenterY = currentChunk.transform.position.y;
        float chunkTop = chunkCenterY + (chunkHeight / 2f);

        if (player.position.x > chunkTop - borderThreshold)
        {
            for (int i = 1; i <= chunksAhead; i++)
            {
                Vector3 spawnPos = currentChunk.transform.position + i * spawnDistance * Vector3.forward;
                if (!ChunkExistsAt(spawnPos))
                {
                    SpawnChunk(spawnPos);
                }
            }
        }
        if (player.position.z > activeChunks.Peek().transform.position.z + chunkHeight + 10f)
        {
            GameObject oldChunk = activeChunks.Dequeue();
            if (oldChunk != null) Destroy(oldChunk);
        }
    }

    private void SpawnChunk(Vector3 spawnPosition)
    {
        GameObject chunkToSpawn;

        if (spawnEndChunkNext && endChunk != null)
        {
            chunkToSpawn = endChunk;
            spawnEndChunkNext = false;
            endChunkSpawned = true;
        }
        else
        {
            if (currentBiomePrefab == null || currentBiomeCount >= maxBiomeLength)
            {
                currentBiomeCount = 0;
                currentBiomePrefab = GetRandomBiomeDifferentFrom(currentBiomePrefab);
            }

            if (currentBiomeCount >= BiomeLength)
            {
                currentBiomePrefab = GetRandomBiomeDifferentFrom(currentBiomePrefab);
                currentBiomeCount = 0;
            }

            chunkToSpawn = currentBiomePrefab;
            currentBiomeCount++;
        }

        GameObject newChunk = Instantiate(chunkToSpawn, spawnPosition, chunkToSpawn.transform.rotation);
        activeChunks.Enqueue(newChunk);


    }

    private GameObject GetRandomBiomeDifferentFrom(GameObject current)
    {
        if (chunkPrefabs.Count <= 1) return chunkPrefabs[0];
        GameObject next;
        do
        {
            next = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        } while (next == current);
        return next;
    }

    private bool ChunkExistsAt(Vector3 position)
    {
        foreach (GameObject chunk in activeChunks)
        {
            if (Vector3.Distance(chunk.transform.position, position) < 0.1f)
                return true;
        }
        return false;
    }

    private GameObject GetCurrentChunk()
    {
        if (activeChunks.Count == 0) return null;
        GameObject closest = null;
        float closestDist = float.MaxValue;
        foreach (GameObject chunk in activeChunks)
        {
            float dist = Mathf.Abs(player.position.y - chunk.transform.position.y);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = chunk;
            }
        }
        return closest;
    }

    public void TriggerEndChunk()
    {
        spawnEndChunkNext = true;
    }
}
