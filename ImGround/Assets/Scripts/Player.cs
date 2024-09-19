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
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool isReady;
    bool isDigReady;
    bool isAttacking = false;
    bool isDigging = false;

    float attackDelay;
    float digDelay;
    int toolIndex = 0;

    public Camera followCamera;
    public GameObject[] tools;
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
        Swap();
    }

    void getInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        fDown = Input.GetButton("Fire1");
        dDown = Input.GetButton("Fire2");
        //sDown1 = Input.GetButtonDown("Swap1");
        //sDown2 = Input.GetButtonDown("Swap2");
        //sDown3 = Input.GetButtonDown("Swap3");
        sDown1 = Input.GetKeyDown(KeyCode.Alpha1); // 1번 키
        sDown2 = Input.GetKeyDown(KeyCode.Alpha2); // 2번 키
        sDown3 = Input.GetKeyDown(KeyCode.Alpha3); // 3번 키
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
        digDelay += Time.deltaTime;
        isReady = 0.4f < attackDelay;
        isDigReady = 1.5f < digDelay;
        if (fDown && isReady && !isDigging)
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
        else if(toolIndex == 1 && dDown && isDigReady && !isAttacking)
        {
            anim.SetTrigger("doDigDown");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
        else if (toolIndex == 2 && dDown && isDigReady && !isAttacking)
        {
            anim.SetTrigger("doDigUp");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
    }
    void Swap()
    {
        int currentIndex = toolIndex;
        if (sDown1)
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 0;
        }
        if (sDown2)
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 1;
        }
        if (sDown3)
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 2;
        }

        tools[toolIndex].gameObject.SetActive(true);
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션이 끝나는 시간 (임의로 설정)
        isAttacking = false;
    }

    IEnumerator ResetDig()
    {
        yield return new WaitForSeconds(1.5f); // 땅파기 애니메이션이 끝나는 시간 (임의로 설정)
        isDigging = false;
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
