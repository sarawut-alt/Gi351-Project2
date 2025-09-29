using UnityEngine;

public class Checkpoint : MonoBehaviour//ใส่ในจุดเช็คพ้อย
{
    public Color activeColor = Color.green; // สีที่จะเปลี่ยนเมื่อ Checkpoint ทำงานแล้ว

    private SpriteRenderer[] renderersToChangeColor;
    private bool isActivated = false;

    private void Awake()
    {
        renderersToChangeColor = GetComponentsInChildren<SpriteRenderer>();
    }

    // ฟังก์ชันนี้จะทำงานเมื่อมี Collider อื่นเข้ามาในพื้นที่ Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่าเป็นผู้เล่น และ Checkpoint นี้ยังไม่เคยทำงาน
        if (other.CompareTag("Player") && !isActivated)
        {
            // ตั้งค่าให้ Checkpoint นี้ทำงานแล้ว
            isActivated = true;

            // อัปเดตตำแหน่งเกิดใหม่ล่าสุดในสคริปต์ PlayerRespawn
            // ให้เป็นตำแหน่งของ Checkpoint อันนี้
            PlayerRespawn.lastCheckpointPosition = transform.position;

            Debug.Log("Checkpoint Activated at: " + transform.position);

            //เปลี่ยนสีเพื่อแสดงให้ผู้เล่นรู้ว่า Checkpoint ทำงานแล้ว
            foreach (SpriteRenderer rend in renderersToChangeColor)
            {
                if (rend != null)
                {
                    rend.color = activeColor;
                }
            }

            
        }
    }
}