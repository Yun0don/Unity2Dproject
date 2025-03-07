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

    private float battleDuration = 10f; // �⺻ 60�� �׽�Ʈ��
    private float startTime;

    public List<GameObject> championPrefabs; // �� è�Ǿ��� ������ ����Ʈ
    private Dictionary<string, GameObject> championPrefabDict = new Dictionary<string, GameObject>();

    public Transform blueTeamRespawnPoint; // ����� ������ ��ġ
    public Transform redTeamRespawnPoint;  // ������ ������ ��ġ

    private List<ChampionData> pickedChampions = new List<ChampionData>(); // ���� ���� è�Ǿ� ���

    [HideInInspector] public bool isBattleFinished = false; // ���� ���� ���� ����
    [HideInInspector] public bool isGoldenKill = false;     // ��� ų ��� ����

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
        InitializeChampionPrefabDictionary(); // ��ųʸ��� ������ ����
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
            Debug.LogError("BanPickData�� ã�� �� �����ϴ�!");
        }
    }

    private void InitializeTeams()
    {
        if (pickedChampions == null || pickedChampions.Count < 4)
        {
            Debug.LogError("������ ������ è�Ǿ��� ������� �ʽ��ϴ�!");
            return;
        }

        for (int i = 0; i < pickedChampions.Count; i++)
        {
            int teamID = (i % 2 == 0) ? 1 : 2; // �����(1), ������(2) ������ ��ġ
            SpawnChampion(pickedChampions[i], teamID);
        }
    }

    private void SpawnChampion(ChampionData data, int teamID)
    {
        Transform respawnPoint = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;

        // ���� ���� ������ ������ ��ġ ��������
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
                Debug.Log($"è�Ǿ� ���� ���� �Ϸ�: {data.championName}, ��: {teamID}, ��ġ: {randomPosition}");
            }
        }
    }

    /// <summary>
    /// Ư�� ���� ���� ������ ������ ��ġ�� �������� �Լ�
    /// </summary>
    private Vector2 GetRandomPositionInArea(Transform spawnArea)
    {
        BoxCollider2D collider = spawnArea.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            Debug.LogError("���� ������ BoxCollider2D�� �����ϴ�!");
            return spawnArea.position; // �⺻ ��ġ ��ȯ
        }

        float x = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
        float y = Random.Range(collider.bounds.min.y, collider.bounds.max.y);
        return new Vector2(x, y);
    }


    private IEnumerator BattleLoop()
    {
        // 60�� ���� ����
        while (Time.time - startTime < battleDuration && !isBattleFinished)
        {
            UpdateTimerUI();
            UpdateScoreUI();
            yield return null;
        }

        if (!isBattleFinished)
        {
            EndBattle(); // 60�� ���� �������� �º� ����
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
            Debug.Log("����� �¸�!");
            isBattleFinished = true;
            GameManager.Instance.AddWin(1);
            // ��ġ�� �������� üũ
            if (GameManager.Instance.IsMatchOver())
            {
                ShowFinalMatchResult(true); // �÷��̾�(�����) ���� ���
            }
            else
            {
                ShowGameResult("Blue Team Win!");
            }
        }
        else if (team2Kills > team1Kills)
        {
            Debug.Log("������ �¸�!");
            isBattleFinished = true;
            GameManager.Instance.AddWin(2);
            if (GameManager.Instance.IsMatchOver())
            {
                ShowFinalMatchResult(false); // �÷��̾�(�����) ���� �й�
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
            Debug.Log("����� ��� ų �¸�!");
            GameManager.Instance.AddWin(1);
            ShowGameResult("Blue Team Win!");
        }
        else if (team2Kills > team1Kills)
        {
            Debug.Log("������ ��� ų �¸�!");
            GameManager.Instance.AddWin(2);
            ShowGameResult("Red Team Win!");
        }
        endPanel.SetActive(true);
        GameManager.Instance.DisableAllBattleAI();
    }

    private void ShowGameResult(string result)
    {
        Debug.Log($"[ShowGameResult] ȣ��� - ���: {result}");

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
        // AI ����
        GameManager.Instance.DisableAllBattleAI();

        // resultPanel Ȱ��ȭ
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

        Debug.Log("���� ��ġ ����! " + (isPlayerWin ? "�÷��̾� ���" : "�÷��̾� �й�"));
    }
}

