using UnityEngine;

public class PlayerRespawn : MonoBehaviour//ใส่ใน player
{
    // static variable จะเก็บค่าไว้ตลอดทั้งเกม และสามารถเข้าถึงได้จากทุกที่
    // เราจะใช้ตัวนี้เพื่อเก็บตำแหน่งของ Checkpoint ล่าสุดที่ผู้เล่นไปถึง
    public static Vector3 lastCheckpointPosition;

    private void Awake()
    {
        // เมื่อเริ่มซีนครั้งแรก หรือเมื่อผู้เล่นยังไม่เคยผ่าน Checkpoint ไหนเลย
        // ให้ตั้งค่าตำแหน่งเกิดเริ่มต้นเป็นตำแหน่งปัจจุบันของผู้เล่น
        if (lastCheckpointPosition == Vector3.zero)
        {
            lastCheckpointPosition = transform.position;
        }

        
    }

    // ฟังก์ชันนี้จะถูกเรียกเมื่อผู้เล่น "ตาย"
    public void RespawnPlayer()
    {
        // ย้ายตำแหน่งผู้เล่นกลับไปที่ Checkpoint ล่าสุด
        transform.position = lastCheckpointPosition;

        
    }
}