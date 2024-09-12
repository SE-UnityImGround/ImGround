using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact };
    public Transform target;
    public NavMeshAgent nav; // Ÿ���� �����ϴ� AI ���� Ŭ����
    public Animator anim;

    private int health;
    public int maxHealth;
    public float damage;
    public bool isDie;
    public bool isChase;

    public float fadeDuration = 3f; // ������� �ð�
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
        // �״� ����� �Ϸ�� ������ ��� (�ִϸ��̼� ���̿� �°� ����)
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
