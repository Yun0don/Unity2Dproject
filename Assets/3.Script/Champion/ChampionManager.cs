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
        championAnimation = GetComponent<ChampionAnimation>();
    }

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
    }

    public void TakeDamage(float damage)
    {
        if (isRespawning || championData == null || championData.health <= 0)
            return;

        float reducedDamage = 100f * (damage / (100f + championData.defense));
        championData.health -= reducedDamage;

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

        // �� �� ų ����
        int enemyTeamID = (teamID == 1) ? 2 : 1;
        IncreaseTeamKills(enemyTeamID);

        // ��� ų ��� üũ
        BattleManager battleManager = FindObjectOfType<BattleManager>();
        if (battleManager != null)
        {
            if (battleManager.isGoldenKill && !battleManager.isBattleFinished)
            {
                // ��� ų ��� ���¿��� ù ų �߻� �� ���� �¸� Ȯ��
                battleManager.EndBattleGoldenKill();
            }
            else
            {
                // �Ϲ� ��Ȳ�̸� UI ����
                battleManager.UpdateScoreUI();
            }
        }

        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("isDead");

        Invoke(nameof(HideCharacter), 1f);
        Invoke(nameof(Respawn), 5f);
    }

    private void HideCharacter()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        isRespawning = false;
        gameObject.SetActive(true);

        Transform spawnArea = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;
        if (spawnArea == null) return;

        BoxCollider2D boxCollider = spawnArea.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Vector2 randomPoint = new Vector2(
                Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y)
            );
            transform.position = randomPoint;
        }

        championData.health = championData.maxhealth;
        Invoke(nameof(SetIdleState), 1f);

        BattleAI ai = GetComponent<BattleAI>();
        if (ai != null)
            ai.StartAttackRoutine();
    }

    private void SetIdleState()
    {
        championAnimation?.PlayIdleAnimation();
    }

    // �� ų ���� (UI���� �д� ������ �����ϰ�)
    public static void IncreaseTeamKills(int teamID)
    {
        if (teamID == 1)
            killsBlueTeam++;
        else
            killsRedTeam++;
    }

    // �� ų ��ȸ
    public static int GetTeamKills(int team)
    {
        return (team == 1) ? killsBlueTeam : killsRedTeam;
    }
    public static void ResetTeamKills()
    {
        killsBlueTeam = 0;
        killsRedTeam = 0;
        Debug.Log("�� ���� ų ���ھ �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
