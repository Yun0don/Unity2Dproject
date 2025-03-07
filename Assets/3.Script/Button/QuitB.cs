using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitB : MonoBehaviour
{
    public void OnClickQuitGame()
    {
        // 에디터 환경에서는 종료되지 않으므로
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
