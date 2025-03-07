using System.Collections.Generic;
using UnityEngine;

public class BanPickData : MonoBehaviour
{
    public static BanPickData Instance { get; private set; }

    public List<ChampionData> pickedChampions = new List<ChampionData>();
    public List<Sprite> pickedChampionImages = new List<Sprite>(); 

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

    public void SetPickedChampions(List<ChampionData> champions)
    {
        if (champions == null || champions.Count == 0)
        {
            return;
        }

        pickedChampions = new List<ChampionData>(champions);
        pickedChampionImages.Clear(); 

        foreach (var champion in champions)
        {
            if (champion.championIcon != null)
            {
                pickedChampionImages.Add(champion.championIcon); // è�Ǿ� �̹��� �߰�
            }
        }
    }

    public List<ChampionData> GetPickedChampions()
    {
        return pickedChampions;
    }

    public List<Sprite> GetPickedChampionImages()
    {
        return pickedChampionImages;
    }
}
