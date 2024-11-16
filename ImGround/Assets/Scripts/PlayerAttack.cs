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
    public LayerMask animalLayer;

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
            StartAttack();
            attackDelay = 0f;
            StartCoroutine(ResetAttack());
        }
    }
    public void SpinAttack()
    {
        attackDelay += Time.deltaTime;
        isReady = 2f < attackDelay;
        if (player.pBehavior.dDown && (player.pBehavior.ToolIndex == 6 || player.pBehavior.ToolIndex == 0) && isReady && !player.pBehavior.IsDigging && !player.pBehavior.IsPicking && !player.pBehavior.IsEating && !player.pBehavior.IsPickingUp)
        {
            anim.SetTrigger("doSpinAttack");
            isAttacking = true;
            StartAttack();
            attackDelay = 0f;
            StartCoroutine(ResetSpinAtk());
        }
    }

    private void StartAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        Collider[] hitAnimals = Physics.OverlapSphere(attackPoint.position, attackRange, animalLayer);
        if (hitEnemies.Length > 0)
        {
            foreach (Collider enemy in hitEnemies)
            {
                Enemy enemyHealth = enemy.GetComponent<Enemy>();
                if (enemyHealth != null && !enemyHealth.IsDie)
                {
                    int damage = GetDamageByTool();
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
        if (hitAnimals.Length > 0)
        {
            foreach (Collider animal in hitAnimals)
            {
                Animal animalHealth = animal.GetComponent<Animal>();
                if (animalHealth != null)
                {
                    int damage = GetDamageByTool();
                    animalHealth.TakeDamage(damage);
                }
            }
        }
    }
    // ������ ������ ���
    int GetDamageByTool()
    {
        if (player.pBehavior.ToolIndex == 6) return 5;
        if (player.pBehavior.ToolIndex == 2) return 3;
        if (player.pBehavior.ToolIndex == 0) return 1;
        return 2;
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
    IEnumerator ResetSpinAtk()
    {
        yield return new WaitForSeconds(2f);
        isAttacking = false;
    }
}