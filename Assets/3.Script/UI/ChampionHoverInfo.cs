using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChampionHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ChampionData championData; // è�Ǿ� ������

   

    /// ���콺�� �ø��� è�Ǿ� ���� ǥ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        ChampionInfoUI.UpdateChampionInfo(championData);
    }

    /// ���콺�� ����� ��� ���� ����
    public void OnPointerExit(PointerEventData eventData)
    {
        ChampionInfoUI.HideChampionInfo();
    }


    /// è�Ǿ� ���� �� BanPickManager ȣ��
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{championData.championName} Ŭ����! ���� �õ�");
        BanPickManager.Instance.SelectChampion(championData);
    }
}
