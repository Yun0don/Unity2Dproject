using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> championPrefabs; //  각 챔피언의 프리팹 리스트
    private Dictionary<string, GameObject> championPrefabDict = new Dictionary<string, GameObject>();

    public Transform blueTeamRespawnPoint; // 블루팀 리스폰 위치
    public Transform redTeamRespawnPoint; // 레드팀 리스폰 위치

    private List<ChampionData> pickedChampions = new List<ChampionData>(); // 전투 참가 챔피언 목록

    private void Start()
    {
        LoadPickedChampions();
        InitializeChampionPrefabDictionary(); //  딕셔너리로 프리팹 매핑
        InitializeTeams();
        StartCoroutine(BattleLoop());
    }

    ///  **각 챔피언 데이터와 프리팹을 매칭**
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

    /// **벤픽에서 선택된 챔피언들을 불러옴**
    private void LoadPickedChampions()
    {
        BanPickData banPickData = FindObjectOfType<BanPickData>(); //  싱글톤 대신 `FindObjectOfType()` 사용
        if (banPickData != null)
        {
            pickedChampions = banPickData.GetPickedChampions();
        }
        else
        {
            Debug.LogError("BanPickData를 찾을 수 없습니다!");
        }
    }

    /// **각 팀의 챔피언을 초기화하고 생성**
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

    ///  **딕셔너리를 사용하여 챔피언 생성**
    private void SpawnChampion(ChampionData data, int teamID)
    {
        Transform respawnPoint = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;

        // **해당 챔피언의 프리팹을 Dictionary에서 가져오기**
        if (championPrefabDict.TryGetValue(data.championName, out GameObject prefab))
        {
            GameObject championObj = Instantiate(prefab, respawnPoint.position, Quaternion.identity);
            championObj.name = data.championName;
            ChampionManager champion = championObj.GetComponent<ChampionManager>();

            if (champion != null)
            {
                champion.Initialize(data, teamID, blueTeamRespawnPoint, redTeamRespawnPoint);
                Debug.Log($"챔피언 생성 완료: {data.championName}, 팀: {teamID}");
            }
        }
        else
        {
            Debug.LogError($"'{data.championName}' 챔피언의 프리팹을 찾을 수 없습니다!");
        }
    }

    /// **1분 동안 전투를 진행하는 코루틴**
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

    /// **1분 후 전투가 종료되며 승리 판정을 수행**
    private void EndBattle()
    {
        int team1Kills = ChampionManager.GetTeamKills(1);
        int team2Kills = ChampionManager.GetTeamKills(2);

        if (team1Kills > team2Kills)
        {
            Debug.Log("블루팀 승리!");
        }
        else if (team2Kills > team1Kills)
        {
            Debug.Log("레드팀 승리!");
        }
        else
        {
            Debug.Log("무승부!");
        }
    }
}
