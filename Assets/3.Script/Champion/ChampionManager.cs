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

    /// 챔피언 초기화: 데이터, 팀, 리스폰 위치 설정 후 리스폰 처리
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

    /// 데미지를 받아 체력을 감소시키고, 0 이하일 경우 사망 처리
    public void TakeDamage(float damage)
    {
        if (isRespawning || championData == null || championData.health <= 0)
            return;

        // 방어력 공식 적용 (100 × (damage / (100 + defense)))
        float reducedDamage = 100f * (damage / (100f + championData.defense));
        championData.health -= reducedDamage;

        Debug.Log($"{championData.championName}가 {damage}의 공격을 받았으나, 방어력({championData.defense}) 적용 후 실제 피해: {reducedDamage}. 현재 체력: {championData.health}");

        if (championData.health <= 0)
        {
            Die();
        }
    }

    /// 챔피언 사망 처리: 오브젝트 비활성화 후 일정 시간 뒤 리스폰
    public void Die()
    {
        if (isRespawning) return;

        isRespawning = true;
        if (championData != null)
            championData.health = 0;
        gameObject.SetActive(false);

        // 팀ID가 1이면 상대팀(레드팀)의 킬 수 증가, 아니면 블루팀 킬 수 증가
        if (teamID == 1)
            killsRedTeam++;
        else
            killsBlueTeam++;

        Debug.Log($"{championData.championName} 사망! 5초 후 리스폰...");

        Invoke(nameof(Respawn), 5f);
    }

    /// 챔피언 리스폰: 체력 회복 및 지정된 리스폰 영역 내 랜덤 위치로 이동
    public void Respawn()
    {
        isRespawning = false;
        gameObject.SetActive(true);

        // 리스폰 영역 결정
        Transform spawnArea = (teamID == 1) ? blueTeamRespawnPoint : redTeamRespawnPoint;
        if (spawnArea == null)
        {
            Debug.LogError("리스폰 포인트가 할당되지 않았습니다!");
            return;
        }

        // spawnArea에 BoxCollider2D가 있으면 해당 영역 내 랜덤 위치 계산, 없으면 기본 위치 사용
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
            // BoxCollider2D가 없으면 spawnArea의 위치로 이동
            transform.position = spawnArea.position;
            Debug.LogWarning("BoxCollider2D가 없으므로 spawnArea의 기본 위치로 리스폰합니다.");
        }

        if (championData != null)
            championData.health = championData.maxhealth; // 최대 체력으로 회복

        Debug.Log($"{championData.championName} 리스폰 완료! 위치: {transform.position}");

        // BattleAI의 공격 루틴 재시작
        BattleAI ai = GetComponent<BattleAI>();
        if (ai != null)
        {
            ai.StartAttackRoutine();
        }
    }

    /// 특정 팀의 총 킬 수 반환: 팀ID 1은 블루팀, 아니면 레드팀
    public static int GetTeamKills(int team)
    {
        return (team == 1) ? killsBlueTeam : killsRedTeam;
    }
}
