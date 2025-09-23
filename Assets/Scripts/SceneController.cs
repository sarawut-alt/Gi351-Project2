using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string mianScene = "MainMenu";
    public string lv1Name = "lvl1";
    public string lv2Name = "lvl2";

    #region sigleton
    private static SceneController instance;
    public static SceneController GetInstance()
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
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public void LoadCurrentScene()
    {

    }
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(mianScene);
    }
}