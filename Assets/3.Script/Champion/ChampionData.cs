using UnityEngine;

public enum SkillType
{
    Basic,  // 기본 스킬
    Ultimate // 궁극기
}

/// 스킬 클래스 (기본 스킬 & 궁극기 공통)
[System.Serializable]
public class ChampionSkill
{
    public SkillType skillType;
    public float cooldown;
    public bool isUsed; // 궁극기 사용 여부 (1세트에 1번)
    public Sprite icon; // 스킬 아이콘
    public string description; // 스킬 설명
    public float ultset;
}

/// 챔피언 데이터 (ScriptableObject)
[CreateAssetMenu(fileName = "NewChampion", menuName = "Champion/Create New Champion")]
public class ChampionData : ScriptableObject
{
    public string championName;
    public Sprite championIcon;

    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public float defense;
    public float maxhealth;
    public float health;
    public float movementSpeed;

    public ChampionSkill basicSkill; // 기본 스킬
    public ChampionSkill ultimateSkill; // 궁극기
}
