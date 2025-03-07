using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : BattleAI 
{
    public GameObject arrowPrefab;
    public GameObject arrowPoint;  

    protected override void AttackTarget()
    {
        if (target != null && arrowPrefab != null)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("isAttack");
            }

            Vector3 spawnPos = (arrowPoint != null) ? arrowPoint.transform.position : transform.position;
            GameObject arrowObj = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);
            Arrow arrowScript = arrowObj.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                Vector2 direction = (target.transform.position - spawnPos).normalized;
                arrowScript.Initialize(direction);
            }
        }
    }
}
