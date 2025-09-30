using System.Collections;
using UnityEngine;

public class TutorialUIPopUp : MonoBehaviour
{
    public GameObject UIPopUp;
    public float timer = 5f;

    private void Start()
    {
        UIPopUp.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIPopUp.SetActive(true);
            StartCoroutine(ShowPopUpUI());
        }
    }

    IEnumerator ShowPopUpUI()
    {
        float elapsedTime = 0f; // ตัวแปรนับเวลาที่ผ่านไป
        
        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        UIPopUp.SetActive(false);
        Destroy(gameObject);
    }
}
