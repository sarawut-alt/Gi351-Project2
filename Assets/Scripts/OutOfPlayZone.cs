using UnityEngine;

public class OutOfPlayZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneController.GetInstance().LoadCurrentScene();
        }
    }
}
