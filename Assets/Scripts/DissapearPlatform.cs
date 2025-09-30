using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    // เวลาที่แพลตฟอร์มจะหายไปหลังจากผู้เล่นเหยียบ (หน่วยเป็นวินาที)
    public float disappearDelay = 1f;

    // เวลาที่แพลตฟอร์มจะกลับมาใหม่หลังจากหายไป (หน่วยเป็นวินาที)
    public float reappearDelay = 5f;

    // สีที่จะเปลี่ยนเพื่อเตือนผู้เล่น
    public Color warningColor = Color.red;

    private Collider2D platformCollider;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isTouched = false;

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTouched)
        {
            isTouched = true;
            StartCoroutine(FadeAndDisappear());
        }
    }

    private IEnumerator FadeAndDisappear()
    {
        // --- ส่วนของการเฟดสีเพื่อเตือน ---

        float elapsedTime = 0f; // ตัวแปรนับเวลาที่ผ่านไป
        
        // วนลูปจนกว่าเวลาที่ผ่านไปจะเท่ากับ disappearDelay
        while (elapsedTime < disappearDelay)
        {
            // เพิ่มเวลาที่ผ่านไปในแต่ละเฟรม
            elapsedTime += Time.deltaTime;

            // คำนวณค่า t (0 ถึง 1) เพื่อใช้ในการ Lerp สี
            float t = elapsedTime / disappearDelay;

            // เปลี่ยนสีของ SpriteRenderer โดยค่อยๆ ผสมจากสีเดิม (originalColor) ไปยังสีเตือน (warningColor)
            spriteRenderer.color = Color.Lerp(originalColor, warningColor, t);

            // รอจนถึงเฟรมถัดไปแล้วค่อยทำงานต่อ
            yield return null; 
        }

        // --- ส่วนของการหายไป ---

        // ทำให้แพลตฟอร์มมองไม่เห็นและไม่สามารถชนได้
        spriteRenderer.enabled = false;
        platformCollider.enabled = false;
        SoundManager.Instance.PlaySFX("Cracker");


        // --- ส่วนของการกลับมาใหม่ ---

        // รอ 5 วินาที
        yield return new WaitForSeconds(reappearDelay);
        
        // เปลี่ยนสีกลับเป็นสีดั้งเดิมก่อนแสดงผล
        spriteRenderer.color = originalColor;

        // ทำให้แพลตฟอร์มกลับมามองเห็นและชนได้
        spriteRenderer.enabled = true;
        platformCollider.enabled = true;
        isTouched = false; // รีเซ็ตสถานะ
    }
}