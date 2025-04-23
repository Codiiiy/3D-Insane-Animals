using UnityEngine;
using System.Collections.Generic;

public class SceneryGenerator : MonoBehaviour
{
    public List<GameObject> prefabs;
    public int spawnCount = 10;
    public float spread = 1f;

    private BoxCollider2D[] boxColliders;
    private List<Vector3> spawnedPositions = new List<Vector3>();
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Track spawned objects

    void Start()
    {
        boxColliders = GetComponents<BoxCollider2D>();

        if (boxColliders.Length == 0)
        {
            Debug.LogError("No BoxCollider components found on this GameObject!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnRandomPrefab();
        }
    }

    void SpawnRandomPrefab()
    {
        if (boxColliders.Length == 0 || prefabs.Count == 0) return;

        BoxCollider2D selectedCollider = boxColliders[Random.Range(0, boxColliders.Length)];

        Vector3 center = selectedCollider.bounds.center;
        Vector3 size = selectedCollider.bounds.size;

        Vector3 randomPos = new Vector3(
            Random.Range(center.x - size.x / 2f, center.x + size.x / 2f),
            Random.Range(center.y - size.y / 2f, center.y + size.y / 2f),
            2f
        );

        foreach (Vector3 pos in spawnedPositions)
        {
            if (Vector3.Distance(randomPos, pos) < spread)
                return;
        }

        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
        GameObject spawned = Instantiate(prefab, randomPos, Quaternion.identity);
        spawnedObjects.Add(spawned);
        spawnedPositions.Add(randomPos);
    }

    void OnDestroy()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
    }
}
