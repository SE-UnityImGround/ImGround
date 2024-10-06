using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float attackDelay;
    bool fDown;
    public bool isAttacking = false;
    bool isReady;
    private Player player;
    Animator anim;
    public Transform attackPoint;
    [SerializeField]
    private float attackRange = 1f;
    public LayerMask enemyLayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    public void AttackInput()
    {
        fDown = Input.GetButton("Fire1");
    }
    public void Attack()
    {
        attackDelay += Time.deltaTime;
        isReady = 0.4f < attackDelay;
        if (fDown && isReady && !player.pBehavior.isDigging && !player.pBehavior.isPicking)
        {
            anim.SetTrigger("doAttack");
            isAttacking = true;
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                Enemy enemyHealth = enemy.GetComponent<Enemy>();
                if (enemyHealth != null && !enemyHealth.isDie)
                {
                    enemyHealth.TakeDamage(1);
                }
            }
            attackDelay = 0f;
            StartCoroutine(ResetAttack());
        }
    }
    // ���� ���� �׽�Ʈ�� Ŭ���� (���Ŀ� ���� ����)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f); // ���� �ִϸ��̼��� ������ �ð� (���Ƿ� ����)
        isAttacking = false;
    }
}