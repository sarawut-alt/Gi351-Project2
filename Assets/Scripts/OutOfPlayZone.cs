using UnityEngine;

public class OutOfPlayZone : MonoBehaviour
{
    public Vector3 pos;

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerConntroller = other.gameObject.GetComponent<PlayerController>();
            playerConntroller.transform.position = pos;
            //SceneController.GetInstance().LoadCurrentScene();
        }
    }*/



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ค้นหา Component PlayerRespawn ในตัวผู้เล่นแล้วเรียกฟังก์ชัน RespawnPlayer()
            PlayerRespawn player = other.GetComponent<PlayerRespawn>();
            if (player != null)
            {
                player.RespawnPlayer();
            }
        }
    }
}