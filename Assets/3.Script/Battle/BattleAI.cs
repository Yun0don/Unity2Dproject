using UnityEngine;
using System.Collections;

public class BattleAI : MonoBehaviour
{
    public ChampionManager championManager;
    public GameObject target;
    private SpriteRenderer spriteRenderer;

    private float attackCooldown;
    private bool isMoving = false; 
    private Coroutine attackCoroutine; 
    private Vector2 lastPosition; //  이전 프레임 위치 저장

    private void Start()
    {
        attackCooldown = 1f / championManager.championData.attackSpeed;
        StartAttackRoutine(); 

        spriteRenderer = GetComponent<SpriteRenderer>(); 
        lastPosition = transform.position;
    }

    private void Update()
    {
        SetTarget();

        if (target == null || !target.activeInHierarchy) // ?
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, target.transform.position);
        float attackRange = championManager.championData.attackRange;

        if (distance > attackRange)
        {
            MoveToTarget();
        }
        else if (distance < attackRange * 0.8f)
        {
            Kiting();
        }
        else
        {
            isMoving = false;
        }
        SetFlip();
    }
    private void SetFlip()
    {
        if (target != null)
        {
            float direction = target.transform.position.x - transform.position.x;

            if (direction < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction > 0) 
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
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

        lastPosition = transform.position; 
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
            Vector2 newPosition = Vector2.MoveTowards(transform.position,
                target.transform.position, championManager.championData.movementSpeed * Time.deltaTime);
            // 경기장 제한 코드
            newPosition = ClampPosition(newPosition);
            transform.position = newPosition;
            animator.SetTrigger("isMove");
        }
    }

    private void Kiting()
    {
        if (target != null)
        {
            Animator animator = GetComponent<Animator>();
            isMoving = true;
            animator.SetTrigger("isMove");
            // 후퇴방향, 후퇴지점, 이동~
            Vector2 direction = ((Vector2)transform.position - (Vector2)target.transform.position).normalized; 
            Vector2 desiredPosition = (Vector2)target.transform.position + direction * championManager.championData.attackRange;
            Vector2 newPosition = Vector2.MoveTowards(transform.position, desiredPosition, championManager.championData.movementSpeed * Time.deltaTime);
            // 경기장 제한 코드
            newPosition = ClampPosition(newPosition);
            transform.position = newPosition;
        }
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
    protected virtual void AttackTarget()
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
        }
    }
    private bool IsTargetInRange()
    {
        if (target == null) return false;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        return distance <= championManager.championData.attackRange;
    }
    public void SetTarget()
    {
        float closestDistance = float.MaxValue;
        GameObject closestEnemy = null;

        int myTeam = championManager.teamID;

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
        if (closestEnemy != null && closestEnemy != target)
        {
            target = closestEnemy;
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
}
