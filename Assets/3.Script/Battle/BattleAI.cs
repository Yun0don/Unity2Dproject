using UnityEngine;
using System.Collections;

public class BattleAI : MonoBehaviour
{
    public ChampionManager championManager;
    public GameObject target;
    private SpriteRenderer spriteRenderer;

    private float attackCooldown;
    private bool isMoving = false; // 이동 상태 여부 (애니메이션 제어용)
    private Coroutine attackCoroutine; // 공격 코루틴
    private Vector2 lastPosition; //  이전 프레임 위치 저장

    private void Start()
    {
        attackCooldown = 1f / championManager.championData.attackSpeed;
        StartAttackRoutine(); // 공격 루틴 자동 시작

        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 가져오기
        lastPosition = transform.position; // 초기 위치 저장
    }

    private void Update()
    {
        // 매 프레임마다 타겟 갱신: 이미 타겟이 있어도 주변에서 더 가까운 적이 있는지 확인
        SetTarget();

        if (target == null || !target.activeInHierarchy)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, target.transform.position);
        float attackRange = championManager.championData.attackRange;

        // 적과의 거리에 따라 행동 결정
        if (distance > attackRange)
        {
            // 사거리 밖이면 접근
            MoveToTarget();
        }
        else if (distance < attackRange * 0.8f)
        {
            // 사거리 내에서도 너무 가까우면 kiting (후퇴) 수행
            Kiting();
        }
        else
        {
            // 적절한 거리면 공격 자세 유지
            isMoving = false;
        }
        SetFlip();
    }
    private void SetFlip()
    {
        if (target != null)
        {
            float direction = target.transform.position.x - transform.position.x;

            if (direction < 0) // 타겟이 왼쪽에 있을 때
            {
                spriteRenderer.flipX = true;
            }
            else if (direction > 0) // 타겟이 오른쪽에 있을 때
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            // 이동 방향으로 Flip 조정 (기존 방식)
            float moveDirection = transform.position.x - lastPosition.x;

            if (moveDirection < 0) // 왼쪽 이동
            {
                spriteRenderer.flipX = true;
            }
            else if (moveDirection > 0) // 오른쪽 이동
            {
                spriteRenderer.flipX = false;
            }
        }

        lastPosition = transform.position; // 현재 위치 저장
    }


    private Vector2 ClampPosition(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -5.2f, 5.2f);
        pos.y = Mathf.Clamp(pos.y, -3.2f, 2.6f);
        return pos;
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            Animator animator = GetComponent<Animator>();
            isMoving = true;
            Vector2 newPosition = Vector2.MoveTowards(transform.position, target.transform.position, championManager.championData.movementSpeed * Time.deltaTime);
            newPosition = ClampPosition(newPosition);
            transform.position = newPosition;
            animator.SetTrigger("isMove"); // 공격 애니메이션 실행
        }
    }

    // Kiting: 적과의 거리를 항상 공격 사거리 내 최적 거리로 유지하도록 이동
    private void Kiting()
    {
        if (target != null)
        {
            Animator animator = GetComponent<Animator>();
            isMoving = true;
            animator.SetTrigger("isMove");
            Vector2 direction = ((Vector2)transform.position - (Vector2)target.transform.position).normalized;
            Vector2 desiredPosition = (Vector2)target.transform.position + direction * championManager.championData.attackRange;
            // MoveTowards를 사용해 점진적으로 이동 후 위치 클램프 적용
            Vector2 newPosition = Vector2.MoveTowards(transform.position, desiredPosition, championManager.championData.movementSpeed * Time.deltaTime);
            newPosition = ClampPosition(newPosition);
            transform.position = newPosition;
        }
    }

    private bool IsTargetInRange()
    {
        if (target == null) return false;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        return distance <= championManager.championData.attackRange;
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);
            if (target != null && IsTargetInRange())
            {
                isMoving = false;
                AttackTarget();
            }
        }
    }

    private void AttackTarget()
    {
        if (target != null)
        {
            SetFlip();
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("isAttack"); // 공격 애니메이션 실행
            }

            target.GetComponent<ChampionManager>().TakeDamage(championManager.championData.attackPower);
            Debug.Log($"{championManager.championData.championName}이 {target.name}을 공격!");
        }
    }

    /// 주변의 적들 중 가장 가까운 적을 실시간으로 탐색하여 타겟을 설정
    public void SetTarget()
    {
        float closestDistance = float.MaxValue;
        GameObject closestEnemy = null;

        int myTeam = championManager.teamID;
        // 탐색 범위 (필요에 따라 값 조정)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 10f);

        foreach (var hitCollider in hitColliders)
        {
            ChampionManager enemyChampion = hitCollider.GetComponent<ChampionManager>();

            if (enemyChampion != null && enemyChampion.teamID != myTeam)
            {
                float distance = Vector2.Distance(transform.position, enemyChampion.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemyChampion.gameObject;
                }
            }
        }

        // 타겟이 null이거나 새로운 적이 기존 타겟보다 더 가까울 경우 업데이트
        if (closestEnemy != null && closestEnemy != target)
        {
            target = closestEnemy;
            Debug.Log($"{championManager.championData.championName}가 새로운 타겟으로 {target.name}을 설정!");
        }
    }

    /// 공격 루틴 시작 (부활 후에도 정상적으로 작동)
    public void StartAttackRoutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    // 타겟 테스트 (씬 뷰 디버깅용)
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.transform.position);
            Gizmos.DrawWireSphere(target.transform.position, 0.5f);
        }
    }

    // 공격 사거리 테스트 (씬 뷰 선택 시)
    private void OnDrawGizmosSelected()
    {
        if (championManager != null && championManager.championData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, championManager.championData.attackRange);
        }
    }
}
