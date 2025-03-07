using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI GoldenKillText;
    public TextMeshProUGUI blueTeamScoreText;
    public TextMeshProUGUI redTeamScoreText;
    public TextMeshProUGUI gameResultText;
    public TextMeshProUGUI finalResultText;
   

    public GameObject endPanel;
    public GameObject resultPanel;

    private float battleDuration = 10f; // 기본 60초 테스트중
    private float startTime;

    public List<GameObject> championPrefabs; // 각 챔피언의 프리팹 리스트
    private Dictionary<string, GameObject> championPrefabDict = new Dictionary<string, GameObject>();

    public Transform blueTeamRespawnPoint; // 블루팀 리스폰 위치
    public Transform redTeamRespawnPoint;  // 레드팀 리스폰 위치

    private List<ChampionData> pickedChampions = new List<ChampionData>(); // 전투 참가 챔피언 목록

    [HideInInspector] public bool isBattleFinished = false; // 전투 완전 종료 여부
    [HideInInspector] public bool isGoldenKill = false;     // 골든 킬 모드 여부

    private void Awake()
    {
        if (endPanel != null)
            endPanel.SetActive(false);
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }
    private void Start()
    {
        startTime = Time.time;

        LoadPickedChampions();
        InitializeChampionPrefabDictionary(); // 딕셔너리로 프리팹 매핑
        InitializeTeams();
        StartCoroutine(BattleLoop());
    }

    private void InitializeChampionPrefabDictionary()
    {
        foreach (var prefab in championPrefabs)
        {
            ChampionManager championManager = prefab.GetComponent<ChampionManager>();
            if (championManager != null && championManager.championData != null)
            {
                string championName = championManager.championData.championName;
                if (!championPrefabDict.ContainsKey(championName))
                {
                    championPrefabDict.Add(championName, prefab);
                }
            }
        }
    }

    private void LoadPickedChampions()
    {
        BanPickData banPickData = FindObjectOfType<BanPickData>();
        if (banPickData != null)
        {
            pickedChampions = banPickData.GetPickedChampions();
        }
        else
        {
            Debug.LogError("BanPickData를 찾을 수 없습니다!");
        }
    }

    private void InitializeTeams()
    {
        if (pickedChampions == null || pickedChampions.Count < 4)
        {
            Debug.LogError("전투를 시작할 챔피언이 충분하지 않습니다!");
            return;
        }

        for (int i = 0; i < pickedChampions.Count; i++)
        {
            int teamID = (i % 2 == 0) ? 1 : 2; // 블루팀(1), 레드팀(2) 번갈아 배치
            SpawnChampion(pickedChampions[i], teamID);
        }
    }

    private void SpawnChampion(ChampionData data, int teamID)
    {
        Transform respawnPoint = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;

        // 스폰 영역 내에서 랜덤한 위치 가져오기
        Vector2 randomPosition = GetRandomPositionInArea(respawnPoint);

        if (championPrefabDict.TryGetValue(data.championName, out GameObject prefab))
        {
            GameObject championObj = Instantiate(prefab, randomPosition, Quaternion.identity);
            championObj.name = data.championName;

            ChampionManager champion = championObj.GetComponent<ChampionManager>();
            if (champion != null)
            {
                data.health = data.maxhealth;
                champion.Initialize(data, teamID, blueTeamRespawnPoint, redTeamRespawnPoint);
                Debug.Log($"챔피언 랜덤 생성 완료: {data.championName}, 팀: {teamID}, 위치: {randomPosition}");
            }
        }
    }

    /// <summary>
    /// 특정 스폰 영역 내에서 랜덤한 위치를 가져오는 함수
    /// </summary>
    private Vector2 GetRandomPositionInArea(Transform spawnArea)
    {
        BoxCollider2D collider = spawnArea.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            Debug.LogError("스폰 영역에 BoxCollider2D가 없습니다!");
            return spawnArea.position; // 기본 위치 반환
        }

        float x = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
        float y = Random.Range(collider.bounds.min.y, collider.bounds.max.y);
        return new Vector2(x, y);
    }


    private IEnumerator BattleLoop()
    {
        // 60초 동안 진행
        while (Time.time - startTime < battleDuration && !isBattleFinished)
        {
            UpdateTimerUI();
            UpdateScoreUI();
            yield return null;
        }

        if (!isBattleFinished)
        {
            EndBattle(); // 60초 종료 시점에서 승부 판정
        }
    }

    private void UpdateTimerUI()
    {
        if (!isBattleFinished && !isGoldenKill)
        {
            float remainingTime = Mathf.Max(0, battleDuration - (Time.time - startTime));
            timerText.text = $"{remainingTime:F1}";
        }
    }

    public void UpdateScoreUI()
    {
        int team1Kills = ChampionManager.GetTeamKills(1);
        int team2Kills = ChampionManager.GetTeamKills(2);

        blueTeamScoreText.SetText(team1Kills.ToString());
        redTeamScoreText.SetText(team2Kills.ToString());
    }

    private void EndBattle()
    {
        int team1Kills = ChampionManager.GetTeamKills(1);
        int team2Kills = ChampionManager.GetTeamKills(2);

        if (team1Kills > team2Kills)
        {
            Debug.Log("블루팀 승리!");
            isBattleFinished = true;
            GameManager.Instance.AddWin(1);
            // 매치가 끝났는지 체크
            if (GameManager.Instance.IsMatchOver())
            {
                ShowFinalMatchResult(true); // 플레이어(블루팀) 최종 우승
            }
            else
            {
                ShowGameResult("Blue Team Win!");
            }
        }
        else if (team2Kills > team1Kills)
        {
            Debug.Log("레드팀 승리!");
            isBattleFinished = true;
            GameManager.Instance.AddWin(2);
            if (GameManager.Instance.IsMatchOver())
            {
                ShowFinalMatchResult(false); // 플레이어(블루팀) 최종 패배
            }
            else
            {
                ShowGameResult("Red Team Win!");
            }
        }
        else
        {
            isGoldenKill = true;
            timerText.gameObject.SetActive(false);
            GoldenKillText.gameObject.SetActive(true);
            GoldenKillText.SetText("Golden Kill");
            return;
        }
        endPanel.SetActive(true);
    }

    public void EndBattleGoldenKill()
    {
        if (isBattleFinished) return;

        isBattleFinished = true;
        int team1Kills = ChampionManager.GetTeamKills(1);
        int team2Kills = ChampionManager.GetTeamKills(2);

        if (team1Kills > team2Kills)
        {
            Debug.Log("블루팀 골든 킬 승리!");
            GameManager.Instance.AddWin(1);
            ShowGameResult("Blue Team Win!");
        }
        else if (team2Kills > team1Kills)
        {
            Debug.Log("레드팀 골든 킬 승리!");
            GameManager.Instance.AddWin(2);
            ShowGameResult("Red Team Win!");
        }
        endPanel.SetActive(true);
        GameManager.Instance.DisableAllBattleAI();
    }

    private void ShowGameResult(string result)
    {
        Debug.Log($"[ShowGameResult] 호출됨 - 결과: {result}");

        if (gameResultText != null)
        {
            if (result.Contains("Blue Team Win!"))
            {
                gameResultText.color = Color.blue;
            }
            else if (result.Contains("Red Team Win!"))
            {
                gameResultText.color = Color.red;
            }

            gameResultText.text = result;
            gameResultText.gameObject.SetActive(true);
        }

        timerText.gameObject.SetActive(false);
        GoldenKillText.gameObject.SetActive(false);

        GameManager.Instance.DisableAllBattleAI();
    }
    private void ShowFinalMatchResult(bool isPlayerWin)
    {
        // AI 정지
        GameManager.Instance.DisableAllBattleAI();

        // resultPanel 활성화
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (finalResultText != null)
        {
            if (isPlayerWin)
            {
                finalResultText.text = "You Win!";
                finalResultText.color = Color.blue;
            }
            else
            {
                finalResultText.text = "You Lose!";
                finalResultText.color = Color.red;
            }
        }

        timerText.gameObject.SetActive(false);
        GoldenKillText.gameObject.SetActive(false);

        Debug.Log("최종 매치 종료! " + (isPlayerWin ? "플레이어 우승" : "플레이어 패배"));
    }
}

