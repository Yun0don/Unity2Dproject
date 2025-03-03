using UnityEngine;

public class ChampionAnimation : MonoBehaviour
{
    private Animator animator;
    private BattleAI battleAI;
    private ChampionManager championManager;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        battleAI = GetComponent<BattleAI>();
        championManager = GetComponent<ChampionManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
        {
            Debug.LogWarning($"{gameObject.name}에는 Animator가 없습니다. 애니메이션 실행을 중단합니다.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (battleAI == null || championManager == null) return;

        animator.SetBool("isMove", battleAI.target != null && !IsTargetInRange());
        animator.SetBool("isAttack", battleAI.target != null && IsTargetInRange());
    }
    private bool IsTargetInRange()
    {
        if (battleAI.target == null) return false;
        float distance = Vector2.Distance(transform.position, battleAI.target.transform.position);
        return distance <= championManager.championData.attackRange;
    }
    public void PlayDieAnimation()
    {
        animator.SetBool("isDead", true);
    }

    public void PlayRespawnAnimation()
    {
        animator.SetTrigger("isRespawn");
    }
  
    public void PlayIdleAnimation()
    {
        animator.SetBool("isMove", false);
        animator.SetBool("isAttack", false);
        animator.ResetTrigger("isDead");
        animator.ResetTrigger("isRespawn");
    }

  
}
