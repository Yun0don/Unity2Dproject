using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : BattleAI // BattleAI�� ���
{
    public GameObject arrowPrefab; // ȭ�� ������
    public GameObject arrowPoint;  // ȭ�� �߻� ��ġ (��: Ȱ�� ��)

    protected override void AttackTarget()
    {
        if (target != null && arrowPrefab != null)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("isAttack");
            }

            // arrowPoint�� �����Ǿ� ������ �� ��ġ����, ������ ���� ��ġ���� ����
            Vector3 spawnPos = (arrowPoint != null) ? arrowPoint.transform.position : transform.position;
            GameObject arrowObj = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);
            Arrow arrowScript = arrowObj.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                // ȭ�� �̵� ���� ���: target �������� �߻�
                Vector2 direction = (target.transform.position - spawnPos).normalized;
                arrowScript.Initialize(direction);
            }
        }
    }
}
