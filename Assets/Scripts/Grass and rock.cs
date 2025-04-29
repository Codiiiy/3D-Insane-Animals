using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrassAndRockSpawner : MonoBehaviour
{
    public GameObject[] grassPrefabs;
    public GameObject[] rockPrefabs;
    public GameObject[] treePrefabs;
    public int spawnCount = 100;
    public float checkRadius = 0.2f;
    public float grassSpawnPercentage = 0.6f;
    public float rockSpawnPercentage = 0.3f;
    public float treeSpawnPercentage = 0.1f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        Bounds bounds = renderer.bounds;

        int spawned = 0;
        int attempts = 0;

        while (spawned < spawnCount && attempts < spawnCount * 10)
        {
            attempts++;

            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 position = new Vector3(x, 0f, z);

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

            GameObject prefabToSpawn;
            Quaternion rotation = Quaternion.identity;

            float spawnChance = Random.value;
            if (spawnChance <= grassSpawnPercentage)
            {
                prefabToSpawn = grassPrefabs[Random.Range(0, grassPrefabs.Length)];
            }
            else if (spawnChance <= grassSpawnPercentage + rockSpawnPercentage)
            {
                prefabToSpawn = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                rotation = Quaternion.Euler(
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f)
                );
            }
            else
            {
                prefabToSpawn = treePrefabs[Random.Range(0, treePrefabs.Length)];
                position.y = 1f;
            }

            GameObject decor = Instantiate(prefabToSpawn, position, rotation);
            spawnedObjects.Add(decor);
            spawned++;
        }
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
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

        if (closestPlayer != null)
        {
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
}
