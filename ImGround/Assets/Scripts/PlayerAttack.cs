using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AudioSource[] effectSound; // 효과음 배열
    float attackDelay;
    bool aDown;
    bool isAttacking = false;
    bool isReady;

    public bool IsAttacking { get { return isAttacking; } }

    private Player player;
    Animator anim;
    public Transform attackPoint;
    public Transform[] spinAtkPoint = new Transform[2];
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
        aDown = InputManager.GetButton("Fire1");
    }

    public void Attack()
    {
        attackDelay += Time.deltaTime;
        isReady = 0.8f < attackDelay;
        if (aDown && isReady && !player.pBehavior.IsDigging && !player.pBehavior.IsPicking && !player.pBehavior.IsEating &&
            !player.pBehavior.IsPickingUp && !player.pBehavior.IsHarvest)
        {
            anim.SetTrigger("doAttack");
            isAttacking = true;

            // 공격 효과음 재생
            if (effectSound.Length > 0)
                effectSound[0].Play();

            StartAttack();
            attackDelay = 0f;
            StartCoroutine(ResetAttack());
        }
    }

    public void SpinAttack()
    {
        attackDelay += Time.deltaTime;
        isReady = 2f < attackDelay;
        if (player.level >= 4 && player.pBehavior.dDown && (player.pBehavior.ToolIndex == 6 || player.pBehavior.ToolIndex == 7) && isReady &&
            !player.pBehavior.IsHarvest && !player.pBehavior.IsDigging && !player.pBehavior.IsPicking && !player.pBehavior.IsEating &&
            !player.pBehavior.IsPickingUp)
        {
            int index = player.pBehavior.ToolIndex == 6 ? 0 : 1;
            anim.SetTrigger("doSpinAttack");
            isAttacking = true;

            // 스핀 공격 효과음 재생
            if (effectSound.Length > 0)
                effectSound[1].Play();

            StartAttack();
            attackDelay = 0f;
            StartCoroutine(ResetSpinAtk(index));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            if (enemyHealth != null && !enemyHealth.IsDie)
            {
                int damage = GetDamageByTool();
                enemyHealth.TakeDamage(damage);
            }
        }
        else if (other.tag == "Animal")
        {
            Animal animalHealth = other.GetComponent<Animal>();
            if (animalHealth != null)
            {
                int damage = GetDamageByTool();
                animalHealth.TakeDamage(damage);
                Debug.Log("타격");
            }
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

    // 도구별 데미지 계산
    int GetDamageByTool()
    {
        if (player.pBehavior.ToolIndex == 6) return 5;
        if (player.pBehavior.ToolIndex == 2) return 3;
        if (player.pBehavior.ToolIndex == 0) return 1;
        return 2;
    }


    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션이 끝나는 시간 (임의로 설정)
        isAttacking = false;
    }

    IEnumerator ResetSpinAtk(int index)
    {
        yield return new WaitForSeconds(0.4f);
        spinAtkPoint[index].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        spinAtkPoint[index].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        spinAtkPoint[index].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);

        isAttacking = false;
        spinAtkPoint[index].gameObject.SetActive(false);
    }
}
