using UnityEngine;
using UnityEngine.UI;

public class ChampionInfoUI : MonoBehaviour
{
    public Image championIcon;
    public Text championNameText;
    public Text attackPowerText;
    public Text attackSpeedText;
    public Text attackRangeText;
    public Text healthText;
    public Text defenseText;
    public Text movespeedText;

    public Image basicSkillIcon;
    public Text basicSkillDescription;
    public Text basicSkillCooldown;

    public Image ultimateSkillIcon;
    public Text ultimateSkillDescription;
    public Text ultimateSkillCooldown;

    private static ChampionInfoUI instance;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false); // 시작할 때 UI 숨김
    }

    /// 챔피언 정보 업데이트 (ChampionData 연동)
    public static void UpdateChampionInfo(ChampionData championData)
    {
        if (instance == null || championData == null) return;

        instance.gameObject.SetActive(true); // UI 활성화

        //  기본 챔피언 정보 표시
        instance.championIcon.sprite = championData.championIcon;
        instance.championNameText.text = $"{championData.championName}";
        instance.attackPowerText.text = $"{championData.attackPower}";
        instance.attackSpeedText.text = $"{championData.attackSpeed}";
        instance.attackRangeText.text = $"{championData.attackRange}";
        instance.healthText.text = $"{championData.maxhealth}";
        instance.defenseText.text = $"{championData.defense}";
        instance.movespeedText.text = $"{championData.movementSpeed}";

        //  기본 스킬 정보
        if (championData.basicSkill != null)
        {
            instance.basicSkillIcon.sprite = championData.basicSkill.icon;
            instance.basicSkillDescription.text = $"{championData.basicSkill.description}";
            instance.basicSkillCooldown.text = $"{championData.basicSkill.cooldown}초";
            instance.basicSkillIcon.gameObject.SetActive(true); // 아이콘이 존재하면 활성화
        }
        else
        {
            instance.basicSkillIcon.gameObject.SetActive(false); // 기본 스킬이 없으면 비활성화
        }

        //  궁극기 정보
        if (championData.ultimateSkill != null)
        {
            instance.ultimateSkillIcon.sprite = championData.ultimateSkill.icon;
            instance.ultimateSkillDescription.text = $"{championData.ultimateSkill.description}";
            instance.ultimateSkillCooldown.text = $"{championData.ultimateSkill.ultset}회/SET";
            instance.ultimateSkillIcon.gameObject.SetActive(true);
        }
        else
        {
            instance.ultimateSkillIcon.gameObject.SetActive(false);
        }
    }

    /// 챔피언 정보 숨기기
    public static void HideChampionInfo()
    {
        if (instance != null)
        {
            instance.gameObject.SetActive(false);
        }
    }
}
