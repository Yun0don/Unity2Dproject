using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    // �ν����Ϳ��� 4���� UI Image ������ �Ҵ��ϼ���.
    // �� ����: 
    // - �� 3 (����� ù ��°) �� blueTeamSlot1
    // - �� 4 (������ ù ��°) �� redTeamSlot1
    // - �� 5 (����� �� ��°) �� blueTeamSlot2
    // - �� 6 (������ �� ��°) �� redTeamSlot2
    public Image blueTeamSlot1;
    public Image redTeamSlot1;
    public Image blueTeamSlot2;
    public Image redTeamSlot2;

    private List<ChampionData> pickedChampions = new List<ChampionData>();
    private List<Sprite> pickedChampionImages = new List<Sprite>();

    private void OnEnable()
    {
        // ���� Ȱ��ȭ�� �� BanPickData�� �����͸� �ε��ϰ�, UI�� �����մϴ�.
        LoadPickedChampions();
        SetChampionImages();
    }

    /// <summary>
    /// BanPickData���� ���� è�Ǿ� �����Ϳ� championIcon(Sprite)�� �����ɴϴ�.
    /// </summary>
    private void LoadPickedChampions()
    {
        // BanPickData�� DontDestroyOnLoad�� �����ǹǷ� �� ��ȯ �Ŀ��� ���� �����մϴ�.
        BanPickData banPickData = FindObjectOfType<BanPickData>(); // �Ǵ� BanPickData.Instance ���
        if (banPickData != null)
        {
            pickedChampions = banPickData.GetPickedChampions();
            pickedChampionImages = banPickData.GetPickedChampionImages();
            Debug.Log($"InGameUIManager: è�Ǿ� {pickedChampions.Count}��, �̹��� {pickedChampionImages.Count}�� �ε� �Ϸ�");
        }
        else
        {
            Debug.LogError("BanPickData�� ã�� �� �����ϴ�! è�Ǿ� �����͸� �ε��� �� �����ϴ�.");
        }
    }

    /// <summary>
    /// �ν����Ϳ� �̸� ������ 4���� UI Image ���Կ� ���� è�Ǿ� �̹���(championIcon)�� �Ҵ��մϴ�.
    /// �ȵ� è�Ǿ��� �� ��(1,2��)�� �����ϰ� �� 3,4,5,6 (����Ʈ �ε��� 0~3)�� ����Ǿ� �ִٰ� �����մϴ�.
    /// </summary>
    private void SetChampionImages()
    {
        // �ȵ� è�Ǿ�� �̹����� �ּ� 4�� �̻��̾�� �մϴ�.
        if (pickedChampionImages.Count < 4)
        {
            Debug.LogWarning("è�Ǿ� ������ �Ǵ� �̹����� �����մϴ�! BanPickData���� Ȯ���ϼ���.");
            return;
        }

        if (blueTeamSlot1 != null)
        {
            blueTeamSlot1.sprite = pickedChampionImages[0]; // �� 3
            blueTeamSlot1.gameObject.SetActive(true);
        }
        if (redTeamSlot1 != null)
        {
            redTeamSlot1.sprite = pickedChampionImages[1]; // �� 4
            redTeamSlot1.gameObject.SetActive(true);
        }
        if (blueTeamSlot2 != null)
        {
            blueTeamSlot2.sprite = pickedChampionImages[2]; // �� 5
            blueTeamSlot2.gameObject.SetActive(true);
        }
        if (redTeamSlot2 != null)
        {
            redTeamSlot2.sprite = pickedChampionImages[3]; // �� 6
            redTeamSlot2.gameObject.SetActive(true);
        }

        Debug.Log("è�Ǿ� �̹����� �ΰ��� UI�� ���������� �����Ǿ����ϴ�!");
    }
}
