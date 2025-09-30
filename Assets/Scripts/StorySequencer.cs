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

    [SerializeField] GameObject continueButton;

    private void Start()
    {
        continueButton.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(idx >= cutsceneSprites.Length)
        {
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
        if(idx == cutsceneSprites.Length - 1)
        {
            continueButton.SetActive(true);
        }
    }
    

    IEnumerator AddIdx()
    {
        nextPrepare = false;
        yield return new WaitForSeconds(timeToChangeImg);
        idx++;
        nextPrepare = true;
    }

    public void OnContinueClicked()
    {
        SoundManager.Instance.PlaySFX("UI_Cilck");
        gameObject.SetActive(false);
    }
}
