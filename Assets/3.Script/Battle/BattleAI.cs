using UnityEngine;
using System.Collections;

public class BattleAI : MonoBehaviour
{
    public ChampionManager championManager;
    public GameObject target;

    private float attackCooldown;
    private bool isMoving = false; // �̵� ���� ���� (�ִϸ��̼� �����)
    private Coroutine attackCoroutine; // ���� �ڷ�ƾ

    private void Start()
    {
        attackCooldown = 1f / championManager.championData.attackSpeed;
        StartAttackRoutine(); // ���� ��ƾ �ڵ� ����
    }

    private void Update()
    {
        // �� �����Ӹ��� Ÿ�� ����: �̹� Ÿ���� �־ �ֺ����� �� ����� ���� �ִ��� Ȯ��
        SetTarget();

        if (target == null || !target.activeInHierarchy)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, target.transform.position);
        float attackRange = championManager.championData.attackRange;

        // ������ �Ÿ��� ���� �ൿ ����
        if (distance > attackRange)
        {
            // ��Ÿ� ���̸� ����
            MoveToTarget();
        }
        else if (distance < attackRange * 0.8f)
        {
            // ��Ÿ� �������� �ʹ� ������ kiting (����) ����
            Kiting();
        }
        else
        {
            // ������ �Ÿ��� ���� �ڼ� ����
            isMoving = false;
        }
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
            isMoving = true;
            Vector2 newPosition = Vector2.MoveTowards(transform.position, target.transform.position, championManager.championData.movementSpeed * Time.deltaTime);
            newPosition = ClampPosition(newPosition);
            transform.position = newPosition;
        }
    }

    // Kiting: ������ �Ÿ��� �׻� ���� ��Ÿ� �� ���� �Ÿ��� �����ϵ��� �̵�
    private void Kiting()
    {
        if (target != null)
        {
            isMoving = true;
            Vector2 direction = ((Vector2)transform.position - (Vector2)target.transform.position).normalized;
            Vector2 desiredPosition = (Vector2)target.transform.position + direction * championManager.championData.attackRange;
            // MoveTowards�� ����� ���������� �̵� �� ��ġ Ŭ���� ����
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
                AttackTarget();
            }
        }
    }

    private void AttackTarget()
    {
        if (target != null)
        {
            target.GetComponent<ChampionManager>().TakeDamage(championManager.championData.attackPower);
            Debug.Log($"{championManager.championData.championName}�� {target.name}�� ����!");
        }
    }

    /// <summary>
    /// �ֺ��� ���� �� ���� ����� ���� �ǽð����� Ž���Ͽ� Ÿ���� ����
    /// </summary>
    public void SetTarget()
    {
        float closestDistance = float.MaxValue;
        GameObject closestEnemy = null;

        int myTeam = championManager.teamID;
        // Ž�� ���� (�ʿ信 ���� �� ����)
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

        // Ÿ���� null�̰ų� ���ο� ���� ���� Ÿ�ٺ��� �� ����� ��� ������Ʈ
        if (closestEnemy != null && closestEnemy != target)
        {
            target = closestEnemy;
            Debug.Log($"{championManager.championData.championName}�� ���ο� Ÿ������ {target.name}�� ����!");
        }
    }

    /// <summary>
    /// ���� ��ƾ ���� (��Ȱ �Ŀ��� ���������� �۵�)
    /// </summary>
    public void StartAttackRoutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    // Ÿ�� �׽�Ʈ (�� �� ������)
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.transform.position);
            Gizmos.DrawWireSphere(target.transform.position, 0.5f);
        }
    }

    // ���� ��Ÿ� �׽�Ʈ (�� �� ���� ��)
    private void OnDrawGizmosSelected()
    {
        if (championManager != null && championManager.championData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, championManager.championData.attackRange);
        }
    }
}
