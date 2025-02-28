using System.Collections.Generic;
using UnityEngine;

public class BanPickData : MonoBehaviour
{
    public static BanPickData Instance { get; private set; }

    public List<ChampionData> pickedChampions = new List<ChampionData>(); // ���õ� è�Ǿ� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �Ŀ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    ///  ���ȿ��� ���õ� è�Ǿ� ����
    public void SetPickedChampions(List<ChampionData> champions)
    {
        if (champions == null || champions.Count == 0)
        {
            Debug.LogWarning("è�Ǿ� ����� ��� �ֽ��ϴ�!");
            return;
        }

        pickedChampions = new List<ChampionData>(champions);
        Debug.Log($"BanPickData�� è�Ǿ� ���� �Ϸ�! �� {pickedChampions.Count}��");
    }

    ///  ���� ������ ���õ� è�Ǿ� ��������
    public List<ChampionData> GetPickedChampions()
    {
        return pickedChampions;
    }
}
