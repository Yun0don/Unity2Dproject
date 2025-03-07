using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChampionHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ChampionData championData; 
    public void OnPointerEnter(PointerEventData eventData)
    {
        ChampionInfoUI.UpdateChampionInfo(championData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ChampionInfoUI.HideChampionInfo();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{championData.championName} Ŭ����! ���� �õ�");
        BanPickManager.Instance.SelectChampion(championData);
    }
}
