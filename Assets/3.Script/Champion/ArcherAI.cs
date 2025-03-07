using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : BattleAI // BattleAI를 상속
{
    public GameObject arrowPrefab; // 화살 프리팹
    public GameObject arrowPoint;  // 화살 발사 위치 (예: 활의 끝)

    protected override void AttackTarget()
    {
        if (target != null && arrowPrefab != null)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("isAttack");
            }

            // arrowPoint가 지정되어 있으면 그 위치에서, 없으면 현재 위치에서 생성
            Vector3 spawnPos = (arrowPoint != null) ? arrowPoint.transform.position : transform.position;
            GameObject arrowObj = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);
            Arrow arrowScript = arrowObj.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                // 화살 이동 방향 계산: target 방향으로 발사
                Vector2 direction = (target.transform.position - spawnPos).normalized;
                arrowScript.Initialize(direction);
            }
        }
    }
}
