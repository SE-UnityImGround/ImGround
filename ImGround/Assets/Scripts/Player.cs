using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    [SerializeField]
    private int health = 5;

    bool rDown;
    bool fDown;
    bool dDown;
    bool jDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool sDown5;

    bool isReady;
    bool isDigReady;
    bool isJumpReady;
    bool isPickReady;
    bool isAttacking = false;
    bool isDigging = false;
    bool isJumping = false;
    bool isPicking = false;
    bool isTired = false;
    bool isDie = false;
    bool sleeping = false;

    float attackDelay;
    float jumpDelay;
    float digDelay;
    float pickDelay;
    int toolIndex = 0;

    public Camera followCamera;
    public GameObject[] tools;
    Animator anim;
    Vector3 moveVec;
    public Rigidbody rigid;

    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    public float runningDuration = 3f;
    private float runningTime = 0f;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        getInput();
        if (!isTired)
        {
            Move();
        }
        Turn();
        Attack();
        Swap();
        Jump();
        Sleep();
    }

    void getInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        fDown = Input.GetButton("Fire1");
        dDown = Input.GetButton("Fire2");
        jDown = Input.GetKeyDown(KeyCode.Space);
        sDown1 = Input.GetKeyDown(KeyCode.Alpha1); // 1번 키
        sDown2 = Input.GetKeyDown(KeyCode.Alpha2); // 2번 키
        sDown3 = Input.GetKeyDown(KeyCode.Alpha3); // 3번 키
        sDown4 = Input.GetKeyDown(KeyCode.Alpha4); // 4번 키
        sDown5 = Input.GetKeyDown(KeyCode.Alpha5); // 5번 키
    }

    void Move()
    {
        moveVec = (Quaternion.Euler(0.0f, followCamera.transform.rotation.eulerAngles.y, 0.0f) * new Vector3(hAxis, 0.0f, vAxis)).normalized;
        transform.position += moveVec * speed * (rDown ? 1f : 0.5f) * Time.deltaTime;
      

        bool isWalking = moveVec != Vector3.zero;
        bool isRunning = rDown && moveVec != Vector3.zero;

        // 애니메이션 설정
        anim.SetBool("isWalk", isWalking);
        anim.SetBool("isRun", isRunning);

        if (isRunning)
        {
            runningTime += Time.deltaTime;
            if (runningTime > runningDuration)
            {
                StartCoroutine(Tired());
            }
        }
        else
        {
            runningTime = 0;
        }
    }

    IEnumerator Tired()
    {
        isTired = true;
        anim.SetBool("isTired", true);
        yield return new WaitForSeconds(3f);
        anim.SetBool("isTired", false);
        isTired = false;
    }
    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Attack()
    {
        attackDelay += Time.deltaTime;
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        isReady = 0.4f < attackDelay;
        isDigReady = 1.5f < digDelay;
        isPickReady = 1.2f < pickDelay;
        if (fDown && isReady && !isDigging && !isPicking)
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
        else if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !isAttacking && !isJumping && !isPicking)
        {
            anim.SetTrigger("doDigDown");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
        else if (toolIndex == 2 && dDown && isPickReady && !isAttacking)
        {
            rigid.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            anim.SetTrigger("doPick");
            isPicking = true;
            pickDelay = 0f;
            StartCoroutine(ResetPick());
        }
        else if (toolIndex == 4 && dDown && isDigReady && !isAttacking && !isJumping && !isPicking)
        {
            anim.SetTrigger("doDigUp");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
    }
    void Die()
    {
        anim.SetTrigger("doDie");
        isDie = true;
    }
    void Jump()
    {
        jumpDelay += Time.deltaTime;
        isJumpReady = 1.1f < jumpDelay;

        if(jDown && isJumpReady && !isDigging && !sleeping)
        {
            isJumping = true;
            rigid.AddForce(Vector3.up * 4.5f, ForceMode.Impulse);
            anim.SetTrigger("doJump");
            jumpDelay = 0f;
            StartCoroutine (ResetJump());
        }
    }
    void Sleep()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            sleeping = true;
            isTired = true;
            anim.SetBool("isSleep", true);
        }
        if(sleeping && jDown)
        {
            anim.SetBool("isSleep", false);
            StartCoroutine(ResetSleep());
        }
    }
    IEnumerator ResetSleep()
    {
        yield return new WaitForSeconds(2.3f);
        isTired = false;
        sleeping = false;
    }
    void Swap()
    {
        int currentIndex = toolIndex;
        if (sDown1) // 주먹
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 0;
        }
        if (sDown2) // 괭이
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 1;
        }
        if (sDown3) // 삼지창(과일 수확용)
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 2;
        }
        if (sDown4) // 곡괭이
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 3;
        }
        if (sDown5) // 삽
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 4;
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
    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(1.1f); // 땅파기 애니메이션이 끝나는 시간 (임의로 설정)
        isJumping = false;
    }

    IEnumerator ResetPick()
    {
        yield return new WaitForSeconds(1.2f); // 땅파기 애니메이션이 끝나는 시간 (임의로 설정)
        isPicking = false;
    }
    // 공격 범위 테스트용 클래스 (추후에 삭제 예정)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDie)
        {
            if (other.tag == "fruit" && toolIndex == 2 && isPicking)
            {
                Rigidbody fruitRb = other.GetComponent<Rigidbody>();
                Collider fruitCollider = other.GetComponent<Collider>();
                if (fruitRb != null)
                {
                    fruitRb.useGravity = true;
                    fruitCollider.isTrigger = false;
                }
            }
            else if (other.tag == "BossAttack")
            {
                anim.SetTrigger("doHit");
                health--;
            }
            else if (other.tag == "BossRock")
            {
                anim.SetTrigger("doHit");
                health -= 2;
            }
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
