using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Boss Prefabs")]
    public GameObject stonePrefab; // 돌 프리팹
    public GameObject throwStonePrefab; // 던지는 공격에 사용할 돌 프리펩
    float throwForce = 1000f; // 돌을 던지는 힘
    int numberOfStones = 12; // 스톤 샤워 공격에 사용될 돌의 수
    float radius = 10f; // 스톤 샤워 돌 생성 반경
    public GameObject stonePosition; // 골렘이 꺼내는 돌
    

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
        // 보스 전용 공격 로직
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
        }
        else if (distanceToPlayer <= 15f)
        {
            CreateStoneShower();
            anim.SetTrigger("doStone");
        }
        else
        {
            anim.SetTrigger("doThrow");
            stonePosition.SetActive(true);
            Invoke("ThrowStone", 1.5f);
        }

        yield return new WaitForSeconds(4f); // 보스의 경우 공격 딜레이

        // 플레이어와의 거리가 멀면 추적을 재개
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // 달리는 애니메이션 활성화
        }
        else
        {
            anim.SetBool("isRun", false); // 정지 상태 유지
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
            stonePosition.y = transform.position.y + 15; // 높이 조절
            GameObject stone = Instantiate(stonePrefab, stonePosition, Quaternion.identity);
            Rigidbody rb = stone.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.useGravity = true; // 중력 사용
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
}
