using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitB : MonoBehaviour
{
    public void OnClickQuitGame()
    {
        // ������ ȯ�濡���� ������� �����Ƿ�
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
