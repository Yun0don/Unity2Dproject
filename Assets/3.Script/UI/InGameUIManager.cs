using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    // 인스펙터에서 4개의 UI Image 슬롯을 할당하세요.
    // 픽 순서: 
    // - 픽 3 (블루팀 첫 번째) → blueTeamSlot1
    // - 픽 4 (레드팀 첫 번째) → redTeamSlot1
    // - 픽 5 (블루팀 두 번째) → blueTeamSlot2
    // - 픽 6 (레드팀 두 번째) → redTeamSlot2
    public Image blueTeamSlot1;
    public Image redTeamSlot1;
    public Image blueTeamSlot2;
    public Image redTeamSlot2;

    private List<ChampionData> pickedChampions = new List<ChampionData>();
    private List<Sprite> pickedChampionImages = new List<Sprite>();

    private void OnEnable()
    {
        // 씬이 활성화될 때 BanPickData의 데이터를 로드하고, UI를 설정합니다.
        LoadPickedChampions();
        SetChampionImages();
    }

    /// <summary>
    /// BanPickData에서 픽한 챔피언 데이터와 championIcon(Sprite)을 가져옵니다.
    /// </summary>
    private void LoadPickedChampions()
    {
        // BanPickData는 DontDestroyOnLoad로 유지되므로 씬 전환 후에도 접근 가능합니다.
        BanPickData banPickData = FindObjectOfType<BanPickData>(); // 또는 BanPickData.Instance 사용
        if (banPickData != null)
        {
            pickedChampions = banPickData.GetPickedChampions();
            pickedChampionImages = banPickData.GetPickedChampionImages();
            Debug.Log($"InGameUIManager: 챔피언 {pickedChampions.Count}명, 이미지 {pickedChampionImages.Count}개 로드 완료");
        }
        else
        {
            Debug.LogError("BanPickData를 찾을 수 없습니다! 챔피언 데이터를 로드할 수 없습니다.");
        }
    }

    /// <summary>
    /// 인스펙터에 미리 연결한 4개의 UI Image 슬롯에 픽한 챔피언 이미지(championIcon)를 할당합니다.
    /// 픽된 챔피언은 벤 픽(1,2번)을 제외하고 픽 3,4,5,6 (리스트 인덱스 0~3)로 저장되어 있다고 가정합니다.
    /// </summary>
    private void SetChampionImages()
    {
        // 픽된 챔피언과 이미지가 최소 4개 이상이어야 합니다.
        if (pickedChampionImages.Count < 4)
        {
            Debug.LogWarning("챔피언 데이터 또는 이미지가 부족합니다! BanPickData에서 확인하세요.");
            return;
        }

        if (blueTeamSlot1 != null)
        {
            blueTeamSlot1.sprite = pickedChampionImages[0]; // 픽 3
            blueTeamSlot1.gameObject.SetActive(true);
        }
        if (redTeamSlot1 != null)
        {
            redTeamSlot1.sprite = pickedChampionImages[1]; // 픽 4
            redTeamSlot1.gameObject.SetActive(true);
        }
        if (blueTeamSlot2 != null)
        {
            blueTeamSlot2.sprite = pickedChampionImages[2]; // 픽 5
            blueTeamSlot2.gameObject.SetActive(true);
        }
        if (redTeamSlot2 != null)
        {
            redTeamSlot2.sprite = pickedChampionImages[3]; // 픽 6
            redTeamSlot2.gameObject.SetActive(true);
        }

        Debug.Log("챔피언 이미지가 인게임 UI에 정상적으로 설정되었습니다!");
    }
}
