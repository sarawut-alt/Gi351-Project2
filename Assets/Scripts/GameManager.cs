using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool isWin = false;
    public GameObject winPanel;
    public bool isPause = false;
    public GameObject pausePanel;

    public GameObject[] Strawberry;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winPanel.SetActive(false);
        pausePanel.SetActive(false);
        for(int i = 0; i < Strawberry.Length; i++)
        {
            Strawberry[i].SetActive(true);
        }
    }
    #region sigleton
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }
    #endregion
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isWin)
        {
            winPanel.SetActive(true);
        }
    }
    public void Pause()
    {
        SoundManager.Instance.PlaySFX("UI_Cilck");

        if (!isPause)
        {
            PauseEnable();
            isPause = true;
        }
        else
        {
            PauseDisable();
            isPause = false;
        }
    }
    public void PauseEnable()
    {

        pausePanel.SetActive(true);
        Time.timeScale = 0f;

    }
    public void PauseDisable()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

    }

    public void SetActiveStrawberry(int idx)
    {
        Strawberry[idx].SetActive(false);
    }
}
