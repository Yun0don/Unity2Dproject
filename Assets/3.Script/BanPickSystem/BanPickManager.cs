using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanPickManager : MonoBehaviour
{
    public static BanPickManager Instance { get; private set; }

    public List<ChampionData> availableChampions;
    public List<ChampionData> bannedChampions = new List<ChampionData>();
    public List<ChampionData> pickedChampions = new List<ChampionData>();

    public Image banFirstBlue;
    public Image banFirstRed;
    public Image pickFirstBlue;
    public Image pickFirstRed;
    public Image pickSecondBlue;
    public Image pickSecondRed;

    private int currentStep = 0; // 0~5: 벤픽 진행, 6: 완료

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

    /// 챔피언 선택 로직
    public void SelectChampion(ChampionData champion)
    {
        if (bannedChampions.Contains(champion) || pickedChampions.Contains(champion))
        {
            Debug.Log("이미 선택된 챔피언입니다.");
            return;
        }

        Debug.Log($"{champion.championName} 선택됨! 벤픽 진행 중...");
        BanPickSystem(champion);
    }

    /// 벤픽 시스템 (벤 & 픽 순서대로 진행)
    private void BanPickSystem(ChampionData selectedChampion)
    {
        if (currentStep < 2)
        {
            bannedChampions.Add(selectedChampion);
            ApplyImage((currentStep == 0) ? banFirstBlue : banFirstRed, selectedChampion.championIcon);
            Debug.Log($"{selectedChampion.championName} {(currentStep == 0 ? "블루팀" : "레드팀")} 벤 완료!");
        }
        else
        {
            pickedChampions.Add(selectedChampion);
            switch (currentStep)
            {
                case 2: 
                    ApplyImage(pickFirstBlue, selectedChampion.championIcon); break;
                case 3:
                    
                    ApplyImage(pickFirstRed, selectedChampion.championIcon); break;
                case 4:
                    
                    ApplyImage(pickSecondBlue, selectedChampion.championIcon); break;
                case 5:
                   
                    ApplyImage(pickSecondRed, selectedChampion.championIcon);
                    break;
            }
        }
        currentStep++;

        if (currentStep == 6)
        {
            Debug.Log("벤픽 완료! 전투 씬으로 이동합니다.");

            //  BanPickData에 챔피언 리스트 저장 (전투 씬에서 불러오기 위함)
            if (BanPickData.Instance != null)
            {
                BanPickData.Instance.SetPickedChampions(pickedChampions);
                Debug.Log($"BanPickManager -> BanPickData 저장 완료! 총 {pickedChampions.Count}명");
            }
            //  씬 전환
            SceneChanger.Instance.LoadBattleScene();
        }
   
    }

    private void ApplyImage(Image imageSlot, Sprite championSprite)
    {
        if (imageSlot != null && championSprite != null)
        {
            imageSlot.sprite = championSprite;
            imageSlot.gameObject.SetActive(true); // 이미지 활성화
        }
    }
    public int GetCurrentStep()
    {
        return currentStep;
    }
}
