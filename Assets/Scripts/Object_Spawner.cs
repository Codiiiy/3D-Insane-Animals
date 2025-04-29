using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<GameObject> obstacles;
    public List<GameObject> powerups;
    public int maxObjects = 3;
    public float minDistance = 4f;
    [Range(0, 100)] public int obstacleSpawnChance = 80;

    private float[] railX = new float[] { -3f, 0f, 3f };
    private List<GameObject> activeObjects = new List<GameObject>();

    void Start()
    {
        int attempts = 0;
        while (activeObjects.Count < maxObjects && attempts < 50)
        {
            SpawnObject();
            attempts++;
        }
    }

    void SpawnObject()
    {
        List<GameObject> sourceList = Random.Range(0, 100) < obstacleSpawnChance ? obstacles : powerups;
        if (sourceList.Count == 0) return;

        GameObject prefabToSpawn = sourceList[Random.Range(0, sourceList.Count)];

        Quaternion prefabRotation = prefabToSpawn.transform.rotation;
        Vector3 prefabScale = prefabToSpawn.transform.localScale;
        float y = prefabToSpawn.transform.position.y;
        float x = railX[Random.Range(0, railX.Length)];
        float z = transform.position.z + Random.Range(-4f, 4f);
        Vector3 spawnPos = new Vector3(x, y, z);

        foreach (GameObject obj in activeObjects)
        {
            if (obj != null && Vector3.Distance(obj.transform.position, spawnPos) < minDistance)
            {
                return;
            }
        }

        GameObject newObj = Instantiate(prefabToSpawn, prefabToSpawn.transform.position, prefabRotation);
        newObj.transform.position = spawnPos;
        newObj.transform.localScale = prefabScale;

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
