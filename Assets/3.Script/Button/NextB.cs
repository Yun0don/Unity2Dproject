using UnityEngine;
using UnityEngine.SceneManagement;

public class NextB : MonoBehaviour
{
    public void OnClickNextSet()
    {
        ChampionManager.ResetTeamKills();
        SceneManager.LoadScene("BanPickScene");
    }
}
