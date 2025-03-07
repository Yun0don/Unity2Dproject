using UnityEngine;

public enum SkillType
{
    Basic,  // �⺻ ��ų
    Ultimate // �ñر�
}

/// ��ų Ŭ���� (�⺻ ��ų & �ñر� ����)
[System.Serializable]
public class ChampionSkill
{
    public SkillType skillType;
    public float cooldown;
    public bool isUsed; // �ñر� ��� ���� (1��Ʈ�� 1��)
    public Sprite icon; // ��ų ������
    public string description; // ��ų ����
    public float ultset;
}

/// è�Ǿ� ������ (ScriptableObject)
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

    public ChampionSkill basicSkill; // �⺻ ��ų
    public ChampionSkill ultimateSkill; // �ñر�
}
