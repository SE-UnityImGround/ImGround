using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;

    bool rDown;
    bool fDown;
    bool dDown;
    bool isReady;
    float attackDelay;

    public Camera followCamera;
    Animator anim;
    Vector3 moveVec;

    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        getInput();
        Move();
        Turn();
        Attack();
    }

    void getInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        fDown = Input.GetButton("Fire1");
        dDown = Input.GetButton("Fire2");
    }

    void Move()
    {
        moveVec = (Quaternion.Euler(0.0f, followCamera.transform.rotation.eulerAngles.y, 0.0f) * new Vector3(hAxis, 0.0f, vAxis)).normalized;
        transform.position += moveVec * speed * (rDown ? 1f : 0.5f) * Time.deltaTime;
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Attack()
    {
        attackDelay += Time.deltaTime;
        isReady = 0.4f < attackDelay;
        if (fDown && isReady)
        {
            anim.SetTrigger("doAttack");
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
        }
        else if(dDown && isReady)
        {
            anim.SetTrigger("doDig");
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
}
