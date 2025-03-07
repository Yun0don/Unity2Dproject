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
            DontDestroyOnLoad(gameObject); // �� ���� �� �������� �ʵ��� ����
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