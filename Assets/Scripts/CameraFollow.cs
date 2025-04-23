using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos = new Vector3(0, 8, -15);
    public float followSpeed = 5f;
    public float CameraCenter = 0f;
    public float xRotation = 24f;
    public float CameraHeight = 10f;

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + offsetPos;
        desiredPosition.x = CameraCenter;
        desiredPosition.y = CameraHeight;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
