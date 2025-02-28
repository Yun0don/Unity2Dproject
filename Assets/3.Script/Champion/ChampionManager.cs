using UnityEngine;

public class ChampionManager : MonoBehaviour
{
    public ChampionData championData;
    public Transform blueTeamRespawnPoint;
    public Transform redTeamRespawnPoint;
    [SerializeField] public int teamID { get; private set; }

    private static int killsBlueTeam = 0;
    private static int killsRedTeam = 0;

    private bool isRespawning = false;

    /// è�Ǿ� �ʱ�ȭ: ������, ��, ������ ��ġ ���� �� ������ ó��
    public void Initialize(ChampionData data, int team, Transform blueRespawn, Transform redRespawn)
    {
        if (data == null)
        {
            Debug.LogError("ChampionData�� null�Դϴ�! �ʱ�ȭ ����");
            return;
        }

        championData = data;
        teamID = team;
        blueTeamRespawnPoint = blueRespawn;
        redTeamRespawnPoint = redRespawn;

        Respawn();
    }

    /// �������� �޾� ü���� ���ҽ�Ű��, 0 ������ ��� ��� ó��
    public void TakeDamage(float damage)
    {
        if (isRespawning || championData == null || championData.health <= 0)
            return;

        // ���� ���� ���� (100 �� (damage / (100 + defense)))
        float reducedDamage = 100f * (damage / (100f + championData.defense));
        championData.health -= reducedDamage;

        Debug.Log($"{championData.championName}�� {damage}�� ������ �޾�����, ����({championData.defense}) ���� �� ���� ����: {reducedDamage}. ���� ü��: {championData.health}");

        if (championData.health <= 0)
        {
            Die();
        }
    }

    /// è�Ǿ� ��� ó��: ������Ʈ ��Ȱ��ȭ �� ���� �ð� �� ������
    public void Die()
    {
        if (isRespawning) return;

        isRespawning = true;
        if (championData != null)
            championData.health = 0;
        gameObject.SetActive(false);

        // ��ID�� 1�̸� �����(������)�� ų �� ����, �ƴϸ� ����� ų �� ����
        if (teamID == 1)
            killsRedTeam++;
        else
            killsBlueTeam++;

        Debug.Log($"{championData.championName} ���! 5�� �� ������...");

        Invoke(nameof(Respawn), 5f);
    }

    /// è�Ǿ� ������: ü�� ȸ�� �� ������ ������ ���� �� ���� ��ġ�� �̵�
    public void Respawn()
    {
        isRespawning = false;
        gameObject.SetActive(true);

        // ������ ���� ����
        Transform spawnArea = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;
        if (spawnArea == null)
        {
            Debug.LogError("������ ����Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        // spawnArea�� BoxCollider2D�� ������ �ش� ���� �� ���� ��ġ ���, ������ �⺻ ��ġ ���
        BoxCollider2D boxCollider = spawnArea.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Vector2 randomPoint = new Vector2(
                Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y)
            );
            transform.position = randomPoint;
        }
        else
        {
            // BoxCollider2D�� ������ spawnArea�� ��ġ�� �̵�
            transform.position = spawnArea.position;
            Debug.LogWarning("BoxCollider2D�� �����Ƿ� spawnArea�� �⺻ ��ġ�� �������մϴ�.");
        }

        if (championData != null)
            championData.health = championData.maxhealth; // �ִ� ü������ ȸ��

        Debug.Log($"{championData.championName} ������ �Ϸ�! ��ġ: {transform.position}");

        // BattleAI�� ���� ��ƾ �����
        BattleAI ai = GetComponent<BattleAI>();
        if (ai != null)
        {
            ai.StartAttackRoutine();
        }
    }

    /// Ư�� ���� �� ų �� ��ȯ: ��ID 1�� �����, �ƴϸ� ������
    public static int GetTeamKills(int team)
    {
        return (team == 1) ? killsBlueTeam : killsRedTeam;
    }
}
