using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;


    public float smoothDuration = 0.125f;

    Vector3 desiredPosition;
    Vector3 smoothedPosition;

    void FixedUpdate()
    {
        desiredPosition = player.transform.position + offset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothDuration);
        transform.position = smoothedPosition;
    }
}
