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
        gameObject.SetActive(false); // ������ �� UI ����
    }

    /// è�Ǿ� ���� ������Ʈ (ChampionData ����)
    public static void UpdateChampionInfo(ChampionData championData)
    {
        if (instance == null || championData == null) return;

        instance.gameObject.SetActive(true); // UI Ȱ��ȭ

        //  �⺻ è�Ǿ� ���� ǥ��
        instance.championIcon.sprite = championData.championIcon;
        instance.championNameText.text = $"{championData.championName}";
        instance.attackPowerText.text = $"{championData.attackPower}";
        instance.attackSpeedText.text = $"{championData.attackSpeed}";
        instance.attackRangeText.text = $"{championData.attackRange}";
        instance.healthText.text = $"{championData.maxhealth}";
        instance.defenseText.text = $"{championData.defense}";
        instance.movespeedText.text = $"{championData.movementSpeed}";

        //  �⺻ ��ų ����
        if (championData.basicSkill != null)
        {
            instance.basicSkillIcon.sprite = championData.basicSkill.icon;
            instance.basicSkillDescription.text = $"{championData.basicSkill.description}";
            instance.basicSkillCooldown.text = $"{championData.basicSkill.cooldown}��";
            instance.basicSkillIcon.gameObject.SetActive(true); // �������� �����ϸ� Ȱ��ȭ
        }
        else
        {
            instance.basicSkillIcon.gameObject.SetActive(false); // �⺻ ��ų�� ������ ��Ȱ��ȭ
        }

        //  �ñر� ����
        if (championData.ultimateSkill != null)
        {
            instance.ultimateSkillIcon.sprite = championData.ultimateSkill.icon;
            instance.ultimateSkillDescription.text = $"{championData.ultimateSkill.description}";
            instance.ultimateSkillCooldown.text = $"{championData.ultimateSkill.ultset}ȸ/SET";
            instance.ultimateSkillIcon.gameObject.SetActive(true);
        }
        else
        {
            instance.ultimateSkillIcon.gameObject.SetActive(false);
        }
    }

    /// è�Ǿ� ���� �����
    public static void HideChampionInfo()
    {
        if (instance != null)
        {
            instance.gameObject.SetActive(false);
        }
    }
}
