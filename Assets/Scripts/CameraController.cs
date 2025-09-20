using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;

    //REMINDER may implement smoothing effect
    //public float smoothDuration; 

    Vector3 desiredPosition;

    void FixedUpdate()
    {
        desiredPosition = player.transform.position + offset;

        transform.position = new Vector3(desiredPosition.x , desiredPosition.y , desiredPosition.z);
    }
}
