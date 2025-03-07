using UnityEngine;
using UnityEngine.SceneManagement;

public class LobyB : MonoBehaviour
{
    public void OnClickNextSet()

    {
        GameManager.Instance.ResetMatchScore();
        SceneManager.LoadScene("MainScene");
    }
}
