using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool isWin = false;
    public GameObject winPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winPanel.SetActive(false);
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
}
