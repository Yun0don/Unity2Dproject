using UnityEngine;

public class BanPickUI : MonoBehaviour
{
    public GameObject win_blue1;
    public GameObject win_blue2;
    public GameObject win_red1;
    public GameObject win_red2;

    private void Start()
    {
        UpdateWinLights();
    }

    private void UpdateWinLights()
    {
        int blueWins = GameManager.Instance.blueTeamWins;
        int redWins = GameManager.Instance.redTeamWins;

        win_blue1.SetActive(blueWins >= 1);
        win_blue2.SetActive(blueWins >= 2);

        win_red1.SetActive(redWins >= 1);
        win_red2.SetActive(redWins >= 2);

        if (GameManager.Instance.IsMatchOver())
        {
            Debug.Log("최종 우승팀 결정! 더 이상 게임 진행 없음.");
        }
    }
}
