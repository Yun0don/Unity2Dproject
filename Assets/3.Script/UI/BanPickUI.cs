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

        // 1승 이상이면 win_blue1 활성화, 2승 이상이면 win_blue2 활성화
        win_blue1.SetActive(blueWins >= 1);
        win_blue2.SetActive(blueWins >= 2);

        // 1승 이상이면 win_red1 활성화, 2승 이상이면 win_red2 활성화
        win_red1.SetActive(redWins >= 1);
        win_red2.SetActive(redWins >= 2);

        // 이미 2선승이 난 경우, 더 이상 게임 진행 X
        if (GameManager.Instance.IsMatchOver())
        {
            // 예: 승자가 최종 우승 메시지 출력, 
            // 또는 BanPickScene에서 더 이상 픽 진행하지 않도록 처리
            Debug.Log("최종 우승팀 결정! 더 이상 게임 진행 없음.");
        }
    }
}
