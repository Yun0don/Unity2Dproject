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

    /// **씬 변경 공통 메서드**
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// 메인 메뉴 씬 로드
    public void LoadMainMenu()
    {
        ChangeScene("MainScene");
    }

    /// 벤픽 씬 로드
    public void LoadBanPickScene()
    {
        ChangeScene("BanPickScene");
    }

    /// 전투 씬 로드 (벤픽 완료 후 실행됨)
    public void LoadBattleScene()
    {
        ChangeScene("InGameScene");
    }
}