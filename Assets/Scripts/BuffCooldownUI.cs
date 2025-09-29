using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuffCooldownUI : MonoBehaviour
{

    public Image img;
    public float cooldownTime;
    public bool isStickyActive = false;
    public PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cooldownTime = playerController.stickyEffectTime;
    }

    private void Update()
    {
        if (isStickyActive)
        {
            StopCoroutine(CooldownUI());
            //img.fillAmount = 0;
            StartCoroutine(CooldownUI());
            isStickyActive = false;
        }
    }

    IEnumerator CooldownUI()
    {
        img.fillAmount = 0;

        float elapsedTime = 0f; // ตัวแปรนับเวลาที่ผ่านไป
        while (elapsedTime < cooldownTime)
        {
            // เพิ่มเวลาที่ผ่านไปในแต่ละเฟรม
            elapsedTime += Time.deltaTime;

            // คำนวณค่า t (0 ถึง 1) 
            float t = elapsedTime / cooldownTime;
            float fill = 1 - t;
            img.fillAmount = fill;

            yield return null;
        }
    }
}
