using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Boss Prefabs")]
    public GameObject stonePrefab; // �� ������
    float throwForce = 1000f; // ���� ������ ��
    int numberOfStones = 10; // ���� ���� ���ݿ� ���� ���� ��
    float radius = 7f; // ���� ���� �� ���� �ݰ�
    public GameObject stonePosition; // ���� ������ ��
    

    new void Update()
    {
        base.Update();
    }
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override IEnumerator Attack()
    {
        // ���� ���� ���� ����
        isChase = false;
        nav.isStopped = true;

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDirection);

        isAttack = true;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer <= 6f)
        {
            int ranAction = Random.Range(0, 5);
            if (ranAction <= 3)
            {
                anim.SetTrigger("doPunchA");
                punchPosition.SetActive(true);
                StartCoroutine(ResetPunch());
            }
            else
            {
                CreateStoneShower();
                anim.SetTrigger("doStone");
            }
        }
        else
        {
            anim.SetTrigger("doThrow");
            stonePosition.SetActive(true);
            Invoke("ThrowStone", 1.5f);
        }

        yield return new WaitForSeconds(4f); // ������ ��� ���� ������

        // �÷��̾���� �Ÿ��� �ָ� ������ �簳
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // �޸��� �ִϸ��̼� Ȱ��ȭ
        }
        else
        {
            anim.SetBool("isRun", false); // ���� ���� ����
        }

        isAttack = false;
        nav.isStopped = false;
        isChase = true;
    }
    void CreateStoneShower()
    {
        for (int i = 0; i < numberOfStones; i++)
        {
            Vector3 stonePosition = Random.onUnitSphere * radius + transform.position;
            stonePosition.y = transform.position.y + 12; // ���� ����
            GameObject stone = Instantiate(stonePrefab, stonePosition, Quaternion.identity);
            Rigidbody rb = stone.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.useGravity = true; // �߷� ���
            }
            if (rb.position.y <= 0)
            {
                Destroy(rb);
            }
        }
    }

    void ThrowStone()
    {
        if (stonePrefab && target)
        {
            stonePosition.SetActive(false);
            GameObject stone = Instantiate(stonePrefab, transform.position + Vector3.up, Quaternion.identity);
            Rigidbody rb = stone.GetComponent<Rigidbody>();
            if (rb)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                rb.AddForce(direction * throwForce);
            }

            Destroy(stone, 4f);
        }
    }
    IEnumerator ResetPunch()
    {
        yield return new WaitForSeconds(1.5f);
        punchPosition.SetActive(false);
    }
}
