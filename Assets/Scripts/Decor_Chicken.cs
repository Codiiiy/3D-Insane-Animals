using UnityEngine;

public class Decor_Chicken: MonoBehaviour
{
    public float speed = 20f;
    public float walkDistance = 50f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) >= walkDistance)
        {
            transform.position = startPos;
        }
    }
}
