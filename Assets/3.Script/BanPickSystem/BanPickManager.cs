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

    /// è�Ǿ� ���� ����
    public void SelectChampion(ChampionData champion)
    {
        if (bannedChampions.Contains(champion) || pickedChampions.Contains(champion))
        {
            Debug.Log("�̹� ���õ� è�Ǿ��Դϴ�.");
            return;
        }

        Debug.Log($"{champion.championName} ���õ�! ���� ���� ��...");
        BanPickSystem(champion);
    }

    /// ���� �ý��� (�� & �� ������� ����)
    private void BanPickSystem(ChampionData selectedChampion)
    {
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
            Debug.Log("���� �Ϸ�! ���� ������ �̵��մϴ�.");

            //  BanPickData�� è�Ǿ� ����Ʈ ���� (���� ������ �ҷ����� ����)
            if (BanPickData.Instance != null)
            {
                BanPickData.Instance.SetPickedChampions(pickedChampions);
                Debug.Log($"BanPickManager -> BanPickData ���� �Ϸ�! �� {pickedChampions.Count}��");
            }
            //  �� ��ȯ
            SceneChanger.Instance.LoadBattleScene();
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
}
