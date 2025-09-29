using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StorySequencer : MonoBehaviour
{
    // Image ที่เอาไว้โชว์ภาพ
    [SerializeField] private Image cutsceneImage;
    // Array ของภาพที่จะเล่น
    public Sprite[] cutsceneSprites;

    [SerializeField] private int idx = 0;

    [SerializeField] private float timeToChangeImg = 3.0f;

    [SerializeField] private bool nextPrepare = true;
    private void FixedUpdate()
    {
        if(idx >= cutsceneSprites.Length)
        {
            gameObject.SetActive(false);
            return;
        }
        ShowCurrentStory();
        if (nextPrepare)
        {
            StartCoroutine(AddIdx());
        }
    }
    public void ShowCurrentStory()
    {
        cutsceneImage.sprite = cutsceneSprites[idx];
    }
    

    IEnumerator AddIdx()
    {
        nextPrepare = false;
        yield return new WaitForSeconds(timeToChangeImg);
        idx++;
        nextPrepare = true;
    }
}
