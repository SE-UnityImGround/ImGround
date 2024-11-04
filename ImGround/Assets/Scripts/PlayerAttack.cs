using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float attackDelay;
    bool aDown;
    bool isAttacking = false;
    bool isReady;

    public bool IsAttacking { get { return isAttacking; } }

    private Player player;
    Animator anim;
    public Transform attackPoint;
    [SerializeField]
    private float attackRange = 1.1f;
    public LayerMask enemyLayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    public void AttackInput()
    {
        aDown = Input.GetButton("Fire1");
    }
    public void Attack()
    {
        attackDelay += Time.deltaTime;
        isReady = 0.4f < attackDelay;
        if (aDown && isReady && !player.pBehavior.IsDigging && !player.pBehavior.IsPicking && !player.pBehavior.IsEating && !player.pBehavior.IsPickingUp)
        {
            anim.SetTrigger("doAttack");
            isAttacking = true;
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                Enemy enemyHealth = enemy.GetComponent<Enemy>();
                if (enemyHealth != null && !enemyHealth.IsDie)
                {
                    if (player.pBehavior.ToolIndex == 6)
                    {
                        enemyHealth.TakeDamage(3);
                    }
                    else
                    {
                        enemyHealth.TakeDamage(1);
                    }
                }
            }
            attackDelay = 0f;
            StartCoroutine(ResetAttack());
        }
    }
    // 공격 범위 테스트용 클래스 (추후에 삭제 예정)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션이 끝나는 시간 (임의로 설정)
        isAttacking = false;
    }
}