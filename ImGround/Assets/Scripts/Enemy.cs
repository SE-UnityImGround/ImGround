using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact, Boss };
    public Type type;
    public Transform target;
    public NavMeshAgent nav; // Ÿ���� �����ϴ� AI ���� Ŭ����
    public Animator anim;

    private int health;
    public int maxHealth;
    public float damage;
    public bool isDie;
    public bool isChase;
    public bool isAttack;

    // ======= Fade Parameters =======

    private Renderer fadeRenderer; // ���� �� Fadeout ������ ���� Renderer

    public Shader transparentShader = null; // ���� ������ ������ ������ �ٸ� ���̴�
    public float fadeDuration = 3f; // ������� �ð�

    // ===============================

    private void Start()
    {
        health = maxHealth;

        fadeRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (fadeRenderer != null && transparentShader != null)
            fadeRenderer.material.shader = transparentShader;
    }
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        Invoke("ChaseStart", 2);
    }


    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }
    private void FixedUpdate()
    {
        Targetting();
    }
    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isRun", true);
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
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        // �״� ����� �Ϸ�� ������ ��� (�ִϸ��̼� ���̿� �°� ����)
        yield return new WaitForSeconds(3f);

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
                yield return new WaitForSeconds(0.02f);
            }
        }

        Destroy(gameObject);
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
                    targetRange = 2f;
                    break;
                case Type.Cact:
                    targetRadius = 1f;
                    targetRange = 2f;
                    break;
                case Type.Boss:
                    targetRadius = 3f;
                    targetRange = 3f;
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

    IEnumerator Attack()
    {
        // ������ ���߰� ���� �ִϸ��̼� ����
        isChase = false;
        nav.isStopped = true; // �̵��� ���߱�

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ��
        transform.rotation = Quaternion.LookRotation(lookDirection);

        int ranAction = Random.Range(0, 5);
        isAttack = true;

        if (type == Type.Mush)
        {
            switch (ranAction)
            {
                case 0:
                case 1:
                    anim.SetTrigger("doHeadA");
                    break;
                case 2:
                case 3:
                    anim.SetTrigger("doBodyA");
                    break;
                case 4:
                    anim.SetTrigger("doSpinA");
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
                    break;
                case 3:
                case 4:
                    anim.SetTrigger("doHeadA");
                    break;
            }
        }
        else if(type == Type.Boss)
        {
            anim.SetTrigger("doPunchA");
        }
        
        yield return new WaitForSeconds(2f); // ���� ������
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        // �÷��̾���� �Ÿ��� �ָ� ������ �簳
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // �޸��� �ִϸ��̼� Ȱ��ȭ
        }
        else
        {
            anim.SetBool("isRun", false); // �ȴ� ���� ����
        }

        // ������ ������ �ٽ� ������ ����
        isAttack = false;
        nav.isStopped = false; // �ٽ� �̵� �����ϰ� ����
        isChase = true;
    }
}
