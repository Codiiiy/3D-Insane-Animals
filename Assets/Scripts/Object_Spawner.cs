using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<GameObject> spawnPrefabs;
    public int maxObjects = 3;
    public float minDistance = 4f;
    [Range(0, 100)] public int emptySpawnChance = 50;

    private float[] railX = new float[] { -3f, 0f, 3f };
    private List<GameObject> activeObjects = new List<GameObject>();

    void Start()
    {
        int attempts = 0;
        while (activeObjects.Count < maxObjects && attempts < 50)
        {
            if (Random.Range(0, 100) < emptySpawnChance)
            {
                attempts++;
                continue;
            }

            SpawnObject();
            attempts++;
        }
    }

    void SpawnObject()
    {
        if (spawnPrefabs.Count == 0) return;

        GameObject prefabToSpawn = spawnPrefabs[Random.Range(0, spawnPrefabs.Count)];
        float x = railX[Random.Range(0, railX.Length)];
        float y = transform.position.y + Random.Range(-4f, 4f);
        Vector3 spawnPos = new Vector3(x, y, transform.position.z);

        foreach (GameObject obj in activeObjects)
        {
            if (obj != null && Vector3.Distance(obj.transform.position, spawnPos) < minDistance)
            {
                return;
            }
        }

        GameObject newObj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        activeObjects.Add(newObj);
    }
    void OnDestroy()
    {
        foreach (GameObject obj in activeObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
    }
}
