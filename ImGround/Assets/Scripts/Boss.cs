using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public AudioSource[] BosseffectSound;
    [Header("Boss Prefabs")]
    public GameObject stonePrefab; // �� ������
    public GameObject throwStonePrefab; // ������ ���ݿ� ����� �� ������
    float throwForce = 1000f; // ���� ������ ��
    int numberOfStones = 12; // ���� ���� ���ݿ� ���� ���� ��
    float radius = 10f; // ���� ���� �� ���� �ݰ�
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

        if (distanceToPlayer <= 9f)
        {
            anim.SetTrigger("doPunchA");
            punchPosition.SetActive(true);
            StartCoroutine(ResetPunch());
            if (BosseffectSound.Length > 0 && BosseffectSound[0] != null)
            {
                //BosseffectSound[0].Play();
                StartCoroutine(PlaySoundWithDelay(0.7f));
            }
            else
            {
                Debug.LogError("���� ȿ���� �迭�� ����ְų� 0��° �ε����� null�Դϴ�. ȿ������ ����� �� �����ϴ�.");
            }
        }
        else if (distanceToPlayer <= 15f)
        {
            CreateStoneShower();
            anim.SetTrigger("doStone");
            if (BosseffectSound.Length > 0 && BosseffectSound[1] != null)
            {
                BosseffectSound[1].Play();
            }
            else
            {
                Debug.LogError("���� ȿ���� �迭�� ����ְų� 1��° �ε����� null�Դϴ�. ȿ������ ����� �� �����ϴ�.");
            }
        }
        else
        {
            anim.SetTrigger("doThrow");
            stonePosition.SetActive(true);
            Invoke("ThrowStone", 1.5f);
            if (BosseffectSound.Length > 0 && BosseffectSound[2] != null)
            {
                BosseffectSound[2].Play();
            }
            else
            {
                Debug.LogError("���� ȿ���� �迭�� ����ְų� 2��° �ε����� null�Դϴ�. ȿ������ ����� �� �����ϴ�.");
            }
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
            stonePosition.y = transform.position.y + 15; // ���� ����
            GameObject stone = Instantiate(stonePrefab, stonePosition, Quaternion.identity);
            Rigidbody rb = stone.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.useGravity = true; // �߷� ���
            }
            Destroy(stone, 4f);
        }
    }

    void ThrowStone()
    {
        if (stonePrefab && target)
        {
            
            stonePosition.SetActive(false);
            GameObject stone = Instantiate(throwStonePrefab, transform.position + Vector3.up * 2, Quaternion.identity);
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
    IEnumerator PlaySoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð�(��) ���� ���
        BosseffectSound[0].Play(); // ���� ���
    }
}
