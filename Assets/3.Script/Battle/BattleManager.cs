using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> championPrefabs; //  �� è�Ǿ��� ������ ����Ʈ
    private Dictionary<string, GameObject> championPrefabDict = new Dictionary<string, GameObject>();

    public Transform blueTeamRespawnPoint; // ����� ������ ��ġ
    public Transform redTeamRespawnPoint; // ������ ������ ��ġ

    private List<ChampionData> pickedChampions = new List<ChampionData>(); // ���� ���� è�Ǿ� ���

    private void Start()
    {
        LoadPickedChampions();
        InitializeChampionPrefabDictionary(); //  ��ųʸ��� ������ ����
        InitializeTeams();
        StartCoroutine(BattleLoop());
    }

    ///  **�� è�Ǿ� �����Ϳ� �������� ��Ī**
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

    /// **���ȿ��� ���õ� è�Ǿ���� �ҷ���**
    private void LoadPickedChampions()
    {
        BanPickData banPickData = FindObjectOfType<BanPickData>(); //  �̱��� ��� `FindObjectOfType()` ���
        if (banPickData != null)
        {
            pickedChampions = banPickData.GetPickedChampions();
        }
        else
        {
            Debug.LogError("BanPickData�� ã�� �� �����ϴ�!");
        }
    }

    /// **�� ���� è�Ǿ��� �ʱ�ȭ�ϰ� ����**
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

    ///  **��ųʸ��� ����Ͽ� è�Ǿ� ����**
    private void SpawnChampion(ChampionData data, int teamID)
    {
        Transform respawnPoint = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;

        // **�ش� è�Ǿ��� �������� Dictionary���� ��������**
        if (championPrefabDict.TryGetValue(data.championName, out GameObject prefab))
        {
            GameObject championObj = Instantiate(prefab, respawnPoint.position, Quaternion.identity);
            championObj.name = data.championName;
            ChampionManager champion = championObj.GetComponent<ChampionManager>();

            if (champion != null)
            {
                champion.Initialize(data, teamID, blueTeamRespawnPoint, redTeamRespawnPoint);
                Debug.Log($"è�Ǿ� ���� �Ϸ�: {data.championName}, ��: {teamID}");
            }
        }
        else
        {
            Debug.LogError($"'{data.championName}' è�Ǿ��� �������� ã�� �� �����ϴ�!");
        }
    }

    /// **1�� ���� ������ �����ϴ� �ڷ�ƾ**
    private IEnumerator BattleLoop()
    {
        float battleDuration = 60f;
        float startTime = Time.time;

        while (Time.time - startTime < battleDuration)
        {
            yield return null;
        }

        EndBattle();
    }

    /// **1�� �� ������ ����Ǹ� �¸� ������ ����**
    private void EndBattle()
    {
        int team1Kills = ChampionManager.GetTeamKills(1);
        int team2Kills = ChampionManager.GetTeamKills(2);

        if (team1Kills > team2Kills)
        {
            Debug.Log("����� �¸�!");
        }
        else if (team2Kills > team1Kills)
        {
            Debug.Log("������ �¸�!");
        }
        else
        {
            Debug.Log("���º�!");
        }
    }
}
