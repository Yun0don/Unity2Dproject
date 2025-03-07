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

    private int currentStep = 0; // 0~5: ���� ����, 6: �Ϸ�

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
        Debug.Log($"{champion.championName} ���õ�! ���� Step: {currentStep}");
        BanPickSystem(champion);
    }

    /// ������ ����(1,3,5)�� �� �ڵ����� ���� ��/���� ����
    private void AutoSelectChampionForRedTeam()
    {
        // ���� ���õ��� ���� è�Ǿ�� �� �������� �ϳ� ����
        List<ChampionData> candidates = new List<ChampionData>();
        foreach (var champ in availableChampions)
        {
            // �̹� ���Ǿ��ų� �ȵ� è�Ǿ��� ����
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
        Debug.Log($"[Red AI] {randomChampion.championName} ���� ���� (Step: {currentStep})");
        BanPickSystem(randomChampion);
    }

    /// ���� �ý��� (�� & �� ������� ����)
    /// currentStep�� ���� ��/���� ó���ϰ�, 6�� �Ǹ� �� ��ȯ
    private void BanPickSystem(ChampionData selectedChampion)
    {
        // �� �ܰ�(0,1) �� bannedChampions, �� �ܰ�(2~5) �� pickedChampions
        if (currentStep < 2)
        {
            bannedChampions.Add(selectedChampion);
            ApplyImage((currentStep == 0) ? banFirstBlue : banFirstRed, selectedChampion.championIcon);
            Debug.Log($"{selectedChampion.championName} {(currentStep == 0 ? "�����" : "������")} �� �Ϸ�!");
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

        // ������ ��� ������(6) ���� ������ �̵�
        if (currentStep == 6)
        {
            Debug.Log("���� �Ϸ�! ���� ������ �̵��մϴ�.");

            // BanPickData�� è�Ǿ� ����Ʈ ����
            if (BanPickData.Instance != null)
            {
                BanPickData.Instance.SetPickedChampions(pickedChampions);
                Debug.Log($"BanPickManager -> BanPickData ���� �Ϸ�! �� {pickedChampions.Count}��");
            }
            // �� ��ȯ
            SceneChanger.Instance.LoadBattleScene();
        }
        else
        {
            // ���� ���� �ٲ� currentStep�� ������ ����(1,3,5)��� �ڵ� ����
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
            imageSlot.gameObject.SetActive(true); // �̹��� Ȱ��ȭ
        }
    }

    public int GetCurrentStep()
    {
        return currentStep;
    }
    private IEnumerator AutoSelectChampionForRedTeamDelayed(float delay)
    {
        Debug.Log($"[Red AI] {delay}�� �� ���� ���� ����");
        yield return new WaitForSeconds(delay);
        AutoSelectChampionForRedTeam();
    }
}
