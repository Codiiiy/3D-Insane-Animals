using System.Collections.Generic;
using UnityEngine;

public class GrassAndRockSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] grassPrefabs;
    public GameObject[] rockPrefabs;
    public GameObject[] treePrefabs;

    [Header("Spawn Settings")]
    public int spawnCount = 100;
    public float checkRadius = 0.2f;
    public float grassSpawnPercentage = 0.6f;
    public float rockSpawnPercentage = 0.3f;
    public float treeSpawnPercentage = 0.1f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        // Get the renderer safely
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("No renderer found on " + gameObject.name);
            return;
        }

        Bounds bounds = renderer.bounds;
        int spawned = 0;
        int attempts = 0;

        // Validate prefab arrays
        bool hasGrass = grassPrefabs != null && grassPrefabs.Length > 0;
        bool hasRocks = rockPrefabs != null && rockPrefabs.Length > 0;
        bool hasTrees = treePrefabs != null && treePrefabs.Length > 0;

        if (!hasGrass && !hasRocks && !hasTrees)
        {
            Debug.LogError("No prefabs assigned to spawn!");
            return;
        }

        // Adjust percentages if needed
        if (!hasGrass) grassSpawnPercentage = 0;
        if (!hasRocks) rockSpawnPercentage = 0;
        if (!hasTrees) treeSpawnPercentage = 0;

        float totalPercentage = grassSpawnPercentage + rockSpawnPercentage + treeSpawnPercentage;
        if (totalPercentage <= 0) return;

        // Spawn objects
        while (spawned < spawnCount && attempts < spawnCount * 10)
        {
            attempts++;

            // Generate random position
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 position = new Vector3(x, 0f, z);

            // Check for overlaps
            Collider[] overlaps = Physics.OverlapSphere(position + Vector3.up * 0.5f, checkRadius);
            bool blocked = false;
            foreach (var col in overlaps)
            {
                if (col.gameObject != gameObject)
                {
                    blocked = true;
                    break;
                }
            }
            if (blocked) continue;

            // Determine what to spawn
            GameObject prefabToSpawn = null;
            Quaternion rotation = Quaternion.identity;

            float rand = Random.value * totalPercentage;

            if (rand <= grassSpawnPercentage && hasGrass)
            {
                prefabToSpawn = grassPrefabs[Random.Range(0, grassPrefabs.Length)];
            }
            else if (rand <= grassSpawnPercentage + rockSpawnPercentage && hasRocks)
            {
                prefabToSpawn = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                rotation = Quaternion.Euler(
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f)
                );
            }
            else if (hasTrees)
            {
                prefabToSpawn = treePrefabs[Random.Range(0, treePrefabs.Length)];
                position.y = 1f;
            }

            if (prefabToSpawn != null)
            {
                GameObject decor = Instantiate(prefabToSpawn, position, rotation);
                spawnedObjects.Add(decor);
                spawned++;
            }
        }
    }

    void Update()
    {
        // Find closest player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        GameObject closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        if (closestPlayer == null) return;

        // Cleanup objects behind player
        Vector3 playerPos = closestPlayer.transform.position;
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = spawnedObjects[i];
            if (obj == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            float zDifference = obj.transform.position.z - playerPos.z;
            if (zDifference < -10)
            {
                Destroy(obj);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}
