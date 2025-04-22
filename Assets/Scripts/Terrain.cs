using UnityEngine;

public class FollowZAxis : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = target.position.z;
            transform.position = newPosition;
        }
    }
}
