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
    private ChampionAnimation championAnimation;

    private void Awake()
    {
        championAnimation = GetComponent<ChampionAnimation>(); // ChampionAnimation ����
    }

    // è�Ǿ� �ʱ�ȭ: ������, ��, ������ ��ġ ���� �� ������ ó��
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

    // �������� �޾� ü���� ���ҽ�Ű��, 0 ������ ��� ��� ó��
    public void TakeDamage(float damage)
    {
        if (isRespawning || championData == null || championData.health <= 0)
            return;

        float reducedDamage = 100f * (damage / (100f + championData.defense));
        championData.health -= reducedDamage;

        Debug.Log($"{championData.championName}�� {damage}�� ������ �޾�����, ����({championData.defense}) ���� �� ���� ����: {reducedDamage}. ���� ü��: {championData.health}");

        if (championData.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isRespawning) return;
        isRespawning = true;
        championData.health = 0;

        Debug.Log($"{championData.championName} ���! 5�� �� ������...");

        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("isDead");
       // **��� �ִϸ��̼� ����**
       // championAnimation?.PlayDieAnimation();

        // **1�� �� ĳ���� �����**
        Invoke(nameof(HideCharacter), 1f);

        // **5�� �� ������**
        Invoke(nameof(Respawn), 5f);
    }

    // **ĳ���� ����� (��� �ִϸ��̼� �� ����)**
    private void HideCharacter()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("isRespawn");
        isRespawning = false;
        gameObject.SetActive(true);

        // **������ ��ġ ����**
        Transform spawnArea = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;
        if (spawnArea == null) return;

        // **������ ���� �� ���� ��ġ ����**
        BoxCollider2D boxCollider = spawnArea.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Vector2 randomPoint = new Vector2(
                Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y)
            );
            transform.position = randomPoint;
        }

        // **ü�� �ʱ�ȭ**
        championData.health = championData.maxhealth;
        Debug.Log($"{championData.championName} ������ �Ϸ�! ��ġ: {transform.position}");

        // **��Ȱ �ִϸ��̼� ����**
        // championAnimation?.PlayRespawnAnimation();

        // **1�� �� Idle ���� ��ȯ**
        Invoke(nameof(SetIdleState), 1f);

        // **���� ��ƾ �����**
        BattleAI ai = GetComponent<BattleAI>();
        ai?.StartAttackRoutine();
    }


    private void SetIdleState()
    {
        championAnimation?.PlayIdleAnimation();
    }
    public static int GetTeamKills(int team)
    {
        return (team == 1) ? killsBlueTeam : killsRedTeam;
    }
}
