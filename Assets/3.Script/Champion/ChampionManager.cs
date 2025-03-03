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
        championAnimation = GetComponent<ChampionAnimation>(); // ChampionAnimation 연결
    }

    // 챔피언 초기화: 데이터, 팀, 리스폰 위치 설정 후 리스폰 처리
    public void Initialize(ChampionData data, int team, Transform blueRespawn, Transform redRespawn)
    {
        if (data == null)
        {
            Debug.LogError("ChampionData가 null입니다! 초기화 실패");
            return;
        }
        championData = data;
        teamID = team;
        blueTeamRespawnPoint = blueRespawn;
        redTeamRespawnPoint = redRespawn;
        Respawn();
    }

    // 데미지를 받아 체력을 감소시키고, 0 이하일 경우 사망 처리
    public void TakeDamage(float damage)
    {
        if (isRespawning || championData == null || championData.health <= 0)
            return;

        float reducedDamage = 100f * (damage / (100f + championData.defense));
        championData.health -= reducedDamage;

        Debug.Log($"{championData.championName}가 {damage}의 공격을 받았으나, 방어력({championData.defense}) 적용 후 실제 피해: {reducedDamage}. 현재 체력: {championData.health}");

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

        Debug.Log($"{championData.championName} 사망! 5초 후 리스폰...");

        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("isDead");
       // **사망 애니메이션 실행**
       // championAnimation?.PlayDieAnimation();

        // **1초 후 캐릭터 숨기기**
        Invoke(nameof(HideCharacter), 1f);

        // **5초 후 리스폰**
        Invoke(nameof(Respawn), 5f);
    }

    // **캐릭터 숨기기 (사망 애니메이션 후 실행)**
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

        // **리스폰 위치 설정**
        Transform spawnArea = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;
        if (spawnArea == null) return;

        // **리스폰 영역 내 랜덤 위치 적용**
        BoxCollider2D boxCollider = spawnArea.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Vector2 randomPoint = new Vector2(
                Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y)
            );
            transform.position = randomPoint;
        }

        // **체력 초기화**
        championData.health = championData.maxhealth;
        Debug.Log($"{championData.championName} 리스폰 완료! 위치: {transform.position}");

        // **부활 애니메이션 실행**
        // championAnimation?.PlayRespawnAnimation();

        // **1초 후 Idle 상태 전환**
        Invoke(nameof(SetIdleState), 1f);

        // **전투 루틴 재시작**
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
