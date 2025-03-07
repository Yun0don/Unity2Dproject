using UnityEngine;
using UnityEngine.SceneManagement;

public class StartB : MonoBehaviour
{
    public void OnClickNextSet()
    {
        SceneManager.LoadScene("BanPickScene");
    }
}
