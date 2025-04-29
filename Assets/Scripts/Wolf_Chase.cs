using UnityEngine;

public class Wolf_Chase : MonoBehaviour
{
    public Chicken_movement target;

    public float followDistance = 2f;
    public float followSpeed = 5f;
    public float catchUpDistance = 0.5f;
    private bool catchingUp = false;
    private bool noLives = false;

    void Update()
    {
        if (target == null) return;

        if (noLives)
        {
            transform.position = new Vector3(
                target.transform.position.x,
                target.transform.position.y + 2,
                Mathf.MoveTowards(transform.position.z, target.transform.position.z, followSpeed * Time.deltaTime)
            );

            gameObject.layer = 10;
        }
        else if (catchingUp)
        {
            float targetZ = target.transform.position.z - catchUpDistance;
            transform.position = new Vector3(
                target.transform.position.x,
                target.transform.position.y + 2,
                Mathf.MoveTowards(transform.position.z, targetZ, followSpeed * Time.deltaTime)
            );

            if (transform.position.z != targetZ)
            {
                followSpeed += 1;
            }
        }
        else if (target.isPlaying)
        {
            float targetZ = target.transform.position.z - followDistance;
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, target.transform.position.x, followSpeed * Time.deltaTime),
                transform.position.y,
                Mathf.MoveTowards(transform.position.z, targetZ, followSpeed * Time.deltaTime)
            );
        }
    }

    public void OnHit()
    {
        if (!catchingUp)
        {
            catchingUp = true;
        }
        else
        {
            noLives = true;
        }
    }

    public void TriggerNoLives()
    {
        noLives = true;
    }

    public void LifeUp()
    {
        if (catchingUp)
        {
            catchingUp = false;
        }
    }
}
