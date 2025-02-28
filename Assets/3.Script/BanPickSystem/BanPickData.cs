using System.Collections.Generic;
using UnityEngine;

public class BanPickData : MonoBehaviour
{
    public static BanPickData Instance { get; private set; }

    public List<ChampionData> pickedChampions = new List<ChampionData>(); // 선택된 챔피언 저장

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 후에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    ///  벤픽에서 선택된 챔피언 저장
    public void SetPickedChampions(List<ChampionData> champions)
    {
        if (champions == null || champions.Count == 0)
        {
            Debug.LogWarning("챔피언 목록이 비어 있습니다!");
            return;
        }

        pickedChampions = new List<ChampionData>(champions);
        Debug.Log($"BanPickData에 챔피언 저장 완료! 총 {pickedChampions.Count}명");
    }

    ///  전투 씬에서 선택된 챔피언 가져오기
    public List<ChampionData> GetPickedChampions()
    {
        return pickedChampions;
    }
}
