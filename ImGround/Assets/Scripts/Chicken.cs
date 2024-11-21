using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : Animal
{
    public float runSpeed = 3.0f; // 뛰는 속도
    public float walkSpeed = 1.0f; // 걷는 속도 (다른 동물들과 동일하게 설정)
    public float detectionRadius = 5.0f; // 플레이어 점프 감지 범위
    public Player player;

    void Start()
    {
        // 기본 걷는 속도로 설정
        navAgent.speed = walkSpeed;
        navAgent.updateRotation = false; // 자동 회전 비활성화
    }

    new void Update()
    {
        base.Update(); // Animal 클래스의 Update 메소드 실행
        DetectPlayerJump();
        LookAtPlayer(); // 수동 회전 처리
    }

    // 플레이어의 점프에 놀라서 도망가는 동작
    void DetectPlayerJump()
    {
        // 플레이어의 애니메이터에서 IsJumping 트리거가 활성화되어 있는지 확인
        if (Vector3.Distance(transform.position, target.position) <= detectionRadius &&
            player.pMove.IsJumping) // 플레이어가 점프 중일 때
        {
            navAgent.isStopped = false;
            patrolRadius = 20.0f;
            surprised = true;
            SetNewRandomPatrolTarget();
            anim.SetBool("isRun", true); // 닭이 뛰기 시작함
            navAgent.speed = runSpeed; // 뛰는 속도로 설정
            StartCoroutine(ResetRun());
        }
    }

    IEnumerator ResetRun()
    {
        yield return new WaitForSeconds(4f);
        anim.SetBool("isRun", false);
        surprised = false;
        patrolRadius = 5.0f;
        navAgent.speed = walkSpeed; // 걷는 속도로 복귀
    }

    void LookAtPlayer()
    {
        // 목표 지점으로의 방향 벡터 계산
        Vector3 direction = navAgent.steeringTarget - transform.position;
        direction.y = 0; // Y축 회전은 무시

        // 방향이 존재할 경우 회전 처리
        if (direction != Vector3.zero)
        {
            // 목표 방향을 향해 회전하도록 설정
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 180도 반대로 회전
            Quaternion reversedRotation = targetRotation * Quaternion.Euler(0, 180f, 0);

            // 천천히 회전하도록 Slerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, reversedRotation, Time.deltaTime * 5f);
        }
    }
    public override void Respawn()
    {
        base.Respawn();
    }
}
