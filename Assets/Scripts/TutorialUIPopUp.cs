using System.Collections;
using UnityEngine;

public class TutorialUIPopUp : MonoBehaviour
{
    public GameObject UIPopUp;

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
        float time = 5f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        UIPopUp.SetActive(false);
        Destroy(gameObject);
    }
}
