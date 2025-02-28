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

    /// **�� ���� ���� �޼���**
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// ���� �޴� �� �ε�
    public void LoadMainMenu()
    {
        ChangeScene("MainScene");
    }

    /// ���� �� �ε�
    public void LoadBanPickScene()
    {
        ChangeScene("BanPickScene");
    }

    /// ���� �� �ε� (���� �Ϸ� �� �����)
    public void LoadBattleScene()
    {
        ChangeScene("InGameScene");
    }
}