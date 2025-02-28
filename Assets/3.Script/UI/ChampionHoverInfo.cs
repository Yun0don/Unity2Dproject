using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChampionHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ChampionData championData; // 챔피언 데이터

   

    /// 마우스를 올리면 챔피언 정보 표시
    public void OnPointerEnter(PointerEventData eventData)
    {
        ChampionInfoUI.UpdateChampionInfo(championData);
    }

    /// 마우스를 벗어나면 즉시 정보 숨김
    public void OnPointerExit(PointerEventData eventData)
    {
        ChampionInfoUI.HideChampionInfo();
    }


    /// 챔피언 선택 시 BanPickManager 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{championData.championName} 클릭됨! 벤픽 시도");
        BanPickManager.Instance.SelectChampion(championData);
    }
}
