using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int blueTeamWins = 0;
    public int redTeamWins = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddWin(int winningTeamID)
    {
        if (winningTeamID == 1)
        {
            blueTeamWins++;
        }
        else if (winningTeamID == 2)
        {
            redTeamWins++;
        }
    }

    public bool IsMatchOver()
    {
        return (blueTeamWins >= 2 || redTeamWins >= 2);
    }
    public int GetWinningTeam()
    {
        if (blueTeamWins >= 2) return 1;
        if (redTeamWins >= 2) return 2;
        return 0; 
    }
     public void ResetMatchScore()
     {
         blueTeamWins = 0;
         redTeamWins = 0;
     }

    public void DisableAllBattleAI()
    {
        BattleAI[] allAIs = FindObjectsOfType<BattleAI>();
        foreach (BattleAI ai in allAIs)
        {
            ai.enabled = false;
        }
    }
}
