using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact };
    public Transform target;
    public NavMeshAgent nav; // 타켓을 추적하는 AI 관련 클래스
    public Animator anim;

    private int health;
    public int maxHealth;
    public float damage;
    public bool isDie;
    public bool isChase;

    public float fadeDuration = 3f; // 사라지는 시간
    private void Start()
    {
        health = maxHealth;
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
        isChase = false;
        anim.SetTrigger("doDie");
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        // 죽는 모션이 완료될 때까지 대기 (애니메이션 길이에 맞게 조정)
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
