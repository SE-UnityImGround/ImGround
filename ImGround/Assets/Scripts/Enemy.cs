/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact, Boss };
    public Type type;
    public Transform target;
    public NavMeshAgent nav; // Ÿ���� �����ϴ� AI ���� Ŭ����
    public Animator anim;
    public Vector3 respawnPosition; // ������ ��ġ ����

    [Header("Attack Position")]
    public GameObject punchPosition;
    public GameObject headPosition;

    private int health;
    public int maxHealth;
    protected bool isDie;
    protected bool isChase;
    protected bool isAttack;
    public bool isNight = false;
    public bool IsDie {  get { return isDie; } }
    [Header("Item Reward")]
    public GameObject item;
    
    // ======= Fade Parameters =======

    private Renderer fadeRenderer; // ���� �� Fadeout ������ ���� Renderer

    public Shader transparentShader = null; // ���� ������ ������ ������ �ٸ� ���̴�
    private float fadeDuration = 3f; // ������� �ð�

    // ===============================

    private void Start()
    {
        health = maxHealth;

        fadeRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (fadeRenderer != null && transparentShader != null)
            fadeRenderer.material.shader = transparentShader;

        // ������ �� �÷��̾��� �⺻ ������ ��ġ�� ���� ��ġ�� ����(ħ�� �߰��� �� �ڵ�� ���� ����)
        respawnPosition = transform.position;
    }
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if(type != Type.Boss)
            anim.SetBool("isIdle", true);
        else if(type == Type.Boss)
            ChaseStart();
    }


    protected void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
        if (isDie)
        {
            return;
        }
    }
    protected void FixedUpdate()
    {
        if(isChase && !isAttack)
            Targetting();
        if (!isNight && type != Type.Boss)
            ChaseStop();
        else if(isNight && type != Type.Boss && !isAttack)
        {
            ChaseStart();
        }
    }
    void ChaseStart()
    {
        nav.isStopped = false;
        isChase = true;
        if(type != Type.Boss)
            anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);
    }
    void ChaseStop()
    {
        nav.isStopped = true;
        isChase = false;
        StopAllCoroutines();
        anim.SetBool("isIdle", true);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        anim.SetTrigger("doHit");
        if (health <= 0)
            Die();           
    }

    void Die()
    {
        isDie = true;
        StopAllCoroutines();
        isChase = false;
        nav.isStopped = true; // �̵��� ���߱�
        anim.SetTrigger("doDie");
        StartCoroutine(FadeOut());
    }

    void Targetting()
    {
        if (!isDie)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (type)
            {
                case Type.Mush:
                    targetRadius = 1f;
                    targetRange = 1.5f;
                    break;
                case Type.Cact:
                    targetRadius = 1f;
                    targetRange = 1.5f;
                    break;
                case Type.Boss:
                    targetRadius = 6f;
                    targetRange = 6f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                // Player���� �Ÿ� ���
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                // ���� �Ÿ� �̳��� ���� ��쿡�� ����
                if (distanceToPlayer <= targetRange)
                {
                    anim.SetBool("isRun", false);
                    StartCoroutine(Attack());
                }
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        // ������ ���߰� ���� �ִϸ��̼� ����
        isChase = false;
        nav.isStopped = true; // �̵��� ���߱�

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ��
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        isAttack = true;
        
        if(type != Type.Boss) // Boss�� �ƴ� �ٸ� Ÿ���� �� ����
        {
            int ranAction = Random.Range(0, 5);
            if (type == Type.Mush)
            {
                switch (ranAction)
                {
                    case 0:
                    case 1:
                        anim.SetTrigger("doHeadA");
                        StartCoroutine(HeadAttack());
                        break;
                    case 2:
                    case 3:
                        anim.SetTrigger("doBodyA");
                        StartCoroutine(PunchAttack());
                        break;
                    case 4:
                        anim.SetTrigger("doSpinA");
                        StartCoroutine(HeadAttack());
                        break;
                }
            }
            else if (type == Type.Cact)
            {
                switch (ranAction)
                {
                    case 0:
                    case 1:
                    case 2:
                        anim.SetTrigger("doPunchA");
                        StartCoroutine(PunchAttack());
                        break;
                    case 3:
                    case 4:
                        anim.SetTrigger("doHeadA");
                        StartCoroutine(HeadAttack());
                        break;
                }
            }
        }

        // ������ ������ ���� ������ �� �ٽ� ������ ����
        if (type != Type.Boss)
            yield return new WaitForSeconds(3f); // �Ϲ� ������ ��� ���� ������

        // �÷��̾���� �Ÿ��� �ָ� ������ �簳
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // �޸��� �ִϸ��̼� Ȱ��ȭ
        }
        else
        {
            anim.SetBool("isRun", false); // ���� ���� ����
        }

        // ������ ������ �ٽ� ������ ����
        isAttack = false;
        nav.isStopped = false; // �ٽ� �̵� �����ϰ� ����
        isChase = true;
    }

    IEnumerator HeadAttack()
    {
        headPosition.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        headPosition.SetActive(false);
    }

    IEnumerator PunchAttack()
    {
        punchPosition.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        punchPosition.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        // �״� ����� �Ϸ�� ������ ��� (�ִϸ��̼� ���̿� �°� ����)
        yield return new WaitForSeconds(1.4f);

        // ���� �� Fade Out ȿ��
        if (fadeRenderer == null)
            Debug.LogFormat("Enemy_FadeOut : Fade Renderer�� �������� ����");
        else
        {
            if (transparentShader == null)
                Debug.LogFormat("Enemy_FadeOut : �⺻ Transparent Shader�� ����ȭ ó��");

            float elapsedTime = 0.0f;
            Color c = fadeRenderer.material.color;
            while (elapsedTime <= fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                c.a = ((fadeDuration - elapsedTime) / fadeDuration * 1.0f);
                fadeRenderer.material.color = c;
                yield return new WaitForSeconds(0.003f);
            }
        }

        gameObject.SetActive(false);
        GameObject reward = Instantiate(item, transform.position, Quaternion.identity);
    }

    public void Respawn()
    {
        // ü���� �ʱ�ȭ
        health = maxHealth;

        // ������ ��ġ�� �̵�
        transform.position = respawnPosition;

        // ������ ����
        if (fadeRenderer != null)
        {
            Color c = fadeRenderer.material.color;
            c.a = 1.0f; // ������ ����
            fadeRenderer.material.color = c;
        }
        // ��� ���� ����
        isDie = false;
        isChase = true;
        isAttack = false;
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
            nav.isStopped = false; 
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact, Boss };
    public Type type;
    public Transform target;
    public NavMeshAgent nav; // Ÿ���� �����ϴ� AI ���� Ŭ����
    public Animator anim;
    public Vector3 respawnPosition; // ������ ��ġ ����

    [Header("Attack Position")]
    public GameObject punchPosition;
    public GameObject headPosition;

    private int health;
    public int maxHealth;
    protected bool isDie;
    protected bool isChase;
    protected bool isAttack;
    protected bool isNight = false;
    public bool IsDie { get { return isDie; } }
    [Header("Item Reward")]
    public GameObject[] item;

    private DayAndNight dayAndNightScript;

    // ======= Fade Parameters =======

    private Renderer fadeRenderer; // ���� �� Fadeout ������ ���� Renderer

    public Shader transparentShader = null; // ���� ������ ������ ������ �ٸ� ���̴�
    private float fadeDuration = 3f; // ������� �ð�

    // ===============================

    private void Start()
    {
        health = maxHealth;

        fadeRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (fadeRenderer != null && transparentShader != null)
            fadeRenderer.material.shader = transparentShader;

        // ������ �� �÷��̾��� �⺻ ������ ��ġ�� ���� ��ġ�� ����(ħ�� �߰��� �� �ڵ�� ���� ����)
        respawnPosition = transform.position;

        GameObject directionalLight = GameObject.Find("Directional Light");
        if (directionalLight != null)
        {
            dayAndNightScript = directionalLight.GetComponent<DayAndNight>();
        }

    }
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (type != Type.Boss)
            anim.SetBool("isIdle", true);
        else if (type == Type.Boss)
            ChaseStart();
    }


    protected void Update()
    {
       if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
        if (isDie)
        {
            return;
        }

        
        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight; // isNight ���� ��������
        }

    }
    protected void FixedUpdate()
    {
        if (isChase && !isAttack)
            Targetting();
        if (!isNight && type != Type.Boss)
            ChaseStop();
       
        else if (isNight && type != Type.Boss && !isAttack)
        {
            ChaseStart();
        }
    }
    void ChaseStart()
    {
        nav.isStopped = false;
        isChase = true;
        if (type != Type.Boss)
            anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);
    }
    void ChaseStop()
    {
        nav.isStopped = true;
        isChase = false;
        StopAllCoroutines();
        anim.SetBool("isIdle", true);
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
        {
            nav.isStopped = true;
        }
        isChase = false;
        StopAllCoroutines();
        anim.SetBool("isIdle", true);

    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        anim.SetTrigger("doHit");
        if (health <= 0)
            Die();
    }

    void Die()
    {
        isDie = true;
        StopAllCoroutines();
        isChase = false;
        nav.isStopped = true; // �̵��� ���߱�
        anim.SetTrigger("doDie");
        StartCoroutine(FadeOut());
    }

    void Targetting()
    {
        if (!isDie)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (type)
            {
                case Type.Mush:
                    targetRadius = 1f;
                    targetRange = 1.5f;
                    break;
                case Type.Cact:
                    targetRadius = 1f;
                    targetRange = 1.5f;
                    break;
                case Type.Boss:
                    targetRadius = 6f;
                    targetRange = 6f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                // Player���� �Ÿ� ���
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                // ���� �Ÿ� �̳��� ���� ��쿡�� ����
                if (distanceToPlayer <= targetRange)
                {
                    anim.SetBool("isRun", false);
                    StartCoroutine(Attack());
                }
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        // ������ ���߰� ���� �ִϸ��̼� ����
        isChase = false;
        nav.isStopped = true; // �̵��� ���߱�

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ��
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        isAttack = true;

        if (type != Type.Boss) // Boss�� �ƴ� �ٸ� Ÿ���� �� ����
        {
            int ranAction = Random.Range(0, 5);
            if (type == Type.Mush)
            {
                switch (ranAction)
                {
                    case 0:
                    case 1:
                        anim.SetTrigger("doHeadA");
                        StartCoroutine(HeadAttack());
                        break;
                    case 2:
                    case 3:
                        anim.SetTrigger("doBodyA");
                        StartCoroutine(PunchAttack());
                        break;
                    case 4:
                        anim.SetTrigger("doSpinA");
                        StartCoroutine(HeadAttack());
                        break;
                }
            }
            else if (type == Type.Cact)
            {
                switch (ranAction)
                {
                    case 0:
                    case 1:
                    case 2:
                        anim.SetTrigger("doPunchA");
                        StartCoroutine(PunchAttack());
                        break;
                    case 3:
                    case 4:
                        anim.SetTrigger("doHeadA");
                        StartCoroutine(HeadAttack());
                        break;
                }
            }
        }

        // ������ ������ ���� ������ �� �ٽ� ������ ����
        if (type != Type.Boss)
            yield return new WaitForSeconds(3f); // �Ϲ� ������ ��� ���� ������

        // �÷��̾���� �Ÿ��� �ָ� ������ �簳
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // �޸��� �ִϸ��̼� Ȱ��ȭ
        }
        else
        {
            anim.SetBool("isRun", false); // ���� ���� ����
        }

        // ������ ������ �ٽ� ������ ����
        isAttack = false;
        nav.isStopped = false; // �ٽ� �̵� �����ϰ� ����
        isChase = true;
    }

    IEnumerator HeadAttack()
    {
        headPosition.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        headPosition.SetActive(false);
    }

    IEnumerator PunchAttack()
    {
        punchPosition.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        punchPosition.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        // �״� ����� �Ϸ�� ������ ��� (�ִϸ��̼� ���̿� �°� ����)
        yield return new WaitForSeconds(1.4f);

        // ���� �� Fade Out ȿ��
        if (fadeRenderer == null)
            Debug.LogFormat("Enemy_FadeOut : Fade Renderer�� �������� ����");
        else
        {
            if (transparentShader == null)
                Debug.LogFormat("Enemy_FadeOut : �⺻ Transparent Shader�� ����ȭ ó��");

            float elapsedTime = 0.0f;
            Color c = fadeRenderer.material.color;
            while (elapsedTime <= fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                c.a = ((fadeDuration - elapsedTime) / fadeDuration * 1.0f);
                fadeRenderer.material.color = c;
                yield return new WaitForSeconds(0.003f);
            }
        }

        gameObject.SetActive(false);
        if(item.Length <= 1)
            Instantiate(item[0], transform.position, item[0].transform.rotation);
        else
        {
            foreach (GameObject reward in item)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                Instantiate(reward, transform.position + randomOffset, reward.transform.rotation);
            }

        }

    }

    public void Respawn()
    {
        // ü���� �ʱ�ȭ
        health = maxHealth;

        // ������ ��ġ�� �̵�
        transform.position = respawnPosition;

        // ������ ����
        if (fadeRenderer != null)
        {
            Color c = fadeRenderer.material.color;
            c.a = 1.0f; // ������ ����
            fadeRenderer.material.color = c;
        }
        // ��� ���� ����
        isDie = false;
        isChase = true;
        isAttack = false;
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
            nav.isStopped = false;
    }
}
