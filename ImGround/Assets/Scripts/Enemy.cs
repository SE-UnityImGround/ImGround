using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact, Boss };
    public Type type;
    public Transform target;
    public NavMeshAgent nav; // 타켓을 추적하는 AI 관련 클래스
    public Animator anim;

    private int health;
    public int maxHealth;
    public float damage;
    public bool isDie;
    public bool isChase;
    public bool isAttack;

    // ======= Fade Parameters =======

    private Renderer fadeRenderer; // 죽은 후 Fadeout 적용을 위한 Renderer

    public Shader transparentShader = null; // 알파 렌더링 문제시 적용할 다른 쉐이더
    public float fadeDuration = 3f; // 사라지는 시간

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
        nav.isStopped = true; // 이동을 멈추기
        anim.SetTrigger("doDie");
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        // 죽는 모션이 완료될 때까지 대기 (애니메이션 길이에 맞게 조정)
        yield return new WaitForSeconds(3f);

        // 죽은 후 Fade Out 효과
        if (fadeRenderer == null)
            Debug.LogFormat("Enemy_FadeOut : Fade Renderer를 설정하지 못함");
        else
        {
            if (transparentShader == null)
                Debug.LogFormat("Enemy_FadeOut : 기본 Transparent Shader로 투명화 처리");

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
                // Player와의 거리 계산
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                // 일정 거리 이내에 있을 경우에만 공격
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
        // 추적을 멈추고 공격 애니메이션 실행
        isChase = false;
        nav.isStopped = true; // 이동을 멈추기

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; // Y축 회전을 방지하여 수평으로만 회전
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
        
        yield return new WaitForSeconds(2f); // 공격 딜레이
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        // 플레이어와의 거리가 멀면 추적을 재개
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // 달리는 애니메이션 활성화
        }
        else
        {
            anim.SetBool("isRun", false); // 걷는 상태 유지
        }

        // 공격이 끝나면 다시 추적을 시작
        isAttack = false;
        nav.isStopped = false; // 다시 이동 가능하게 설정
        isChase = true;
    }
}
