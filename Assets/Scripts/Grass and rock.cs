using UnityEngine;

public class GrassAndRockSpawner : MonoBehaviour
{
    public GameObject[] grassPrefabs;
    public GameObject[] rockPrefabs;
    public int spawnCount = 100;
    public float checkRadius = 0.2f;

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

            // Only continue if this spot is clear (ignoring the object this script is on)
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

            if (blocked)
            {
                continue;
            }

            GameObject prefabToSpawn;
            Quaternion rotation = Quaternion.identity;

            if (Random.value <= 0.75f)
            {
                prefabToSpawn = grassPrefabs[Random.Range(0, grassPrefabs.Length)];
            }
            else
            {
                prefabToSpawn = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                rotation = Quaternion.Euler(
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f)
                );
            }

            Instantiate(prefabToSpawn, position, rotation);
            spawned++;
        }
    }
}
