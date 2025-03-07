using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    public void SelectChampion(ChampionData champion)
    {
        if (bannedChampions.Contains(champion) || pickedChampions.Contains(champion))
        {
            return;
        }
        Debug.Log($"{champion.championName} 선택됨! 현재 Step: {currentStep}");
        BanPickSystem(champion);
    }

    /// 레드팀 차례(1,3,5)일 때 자동으로 랜덤 벤/픽을 수행
    private void AutoSelectChampionForRedTeam()
    {
        // 아직 선택되지 않은 챔피언들 중 랜덤으로 하나 선택
        List<ChampionData> candidates = new List<ChampionData>();
        foreach (var champ in availableChampions)
        {
            // 이미 벤되었거나 픽된 챔피언은 제외
            if (!bannedChampions.Contains(champ) && !pickedChampions.Contains(champ))
            {
                candidates.Add(champ);
            }
        }

        if (candidates.Count == 0)
        {
            return;
        }

        ChampionData randomChampion = candidates[Random.Range(0, candidates.Count)];
        Debug.Log($"[Red AI] {randomChampion.championName} 랜덤 선택 (Step: {currentStep})");
        BanPickSystem(randomChampion);
    }

    /// 벤픽 시스템 (벤 & 픽 순서대로 진행)
    /// currentStep에 따라 벤/픽을 처리하고, 6이 되면 씬 전환
    private void BanPickSystem(ChampionData selectedChampion)
    {
        // 벤 단계(0,1) → bannedChampions, 픽 단계(2~5) → pickedChampions
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
                case 2: ApplyImage(pickFirstBlue, selectedChampion.championIcon); break;
                case 3: ApplyImage(pickFirstRed, selectedChampion.championIcon); break;
                case 4: ApplyImage(pickSecondBlue, selectedChampion.championIcon); break;
                case 5: ApplyImage(pickSecondRed, selectedChampion.championIcon); break;
            }
        }

        currentStep++;

        // 벤픽이 모두 끝나면(6) 전투 씬으로 이동
        if (currentStep == 6)
        {
            Debug.Log("벤픽 완료! 전투 씬으로 이동합니다.");

            // BanPickData에 챔피언 리스트 저장
            if (BanPickData.Instance != null)
            {
                BanPickData.Instance.SetPickedChampions(pickedChampions);
                Debug.Log($"BanPickManager -> BanPickData 저장 완료! 총 {pickedChampions.Count}명");
            }
            // 씬 전환
            SceneChanger.Instance.LoadBattleScene();
        }
        else
        {
            // 만약 새로 바뀐 currentStep이 레드팀 차례(1,3,5)라면 자동 선택
            if (currentStep == 1 || currentStep == 3 || currentStep == 5)
            {
                StartCoroutine(AutoSelectChampionForRedTeamDelayed(2f));
            }
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
    private IEnumerator AutoSelectChampionForRedTeamDelayed(float delay)
    {
        Debug.Log($"[Red AI] {delay}초 후 랜덤 선택 시작");
        yield return new WaitForSeconds(delay);
        AutoSelectChampionForRedTeam();
    }
}
