using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경 시 삭제되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadMainMenu()
    {
        ChangeScene("MainScene");
    }
    public void LoadBanPickScene()
    {
        ChangeScene("BanPickScene");
    }
    public void LoadBattleScene()
    {
        ChangeScene("InGameScene");
    }
}