using UnityEngine;

public class Strawberry : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().AddStrawberry();
            Destroy(gameObject);
        }
    }
}
