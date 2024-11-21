using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : Animal
{
    public float runSpeed = 3.0f; // �ٴ� �ӵ�
    public float walkSpeed = 1.0f; // �ȴ� �ӵ� (�ٸ� ������� �����ϰ� ����)
    public float detectionRadius = 5.0f; // �÷��̾� ���� ���� ����
    public Player player;

    void Start()
    {
        // �⺻ �ȴ� �ӵ��� ����
        navAgent.speed = walkSpeed;
        navAgent.updateRotation = false; // �ڵ� ȸ�� ��Ȱ��ȭ
    }

    new void Update()
    {
        base.Update(); // Animal Ŭ������ Update �޼ҵ� ����
        DetectPlayerJump();
        LookAtPlayer(); // ���� ȸ�� ó��
    }

    // �÷��̾��� ������ ��� �������� ����
    void DetectPlayerJump()
    {
        // �÷��̾��� �ִϸ����Ϳ��� IsJumping Ʈ���Ű� Ȱ��ȭ�Ǿ� �ִ��� Ȯ��
        if (Vector3.Distance(transform.position, target.position) <= detectionRadius &&
            player.pMove.IsJumping) // �÷��̾ ���� ���� ��
        {
            navAgent.isStopped = false;
            patrolRadius = 20.0f;
            surprised = true;
            SetNewRandomPatrolTarget();
            anim.SetBool("isRun", true); // ���� �ٱ� ������
            navAgent.speed = runSpeed; // �ٴ� �ӵ��� ����
            StartCoroutine(ResetRun());
        }
    }

    IEnumerator ResetRun()
    {
        yield return new WaitForSeconds(4f);
        anim.SetBool("isRun", false);
        surprised = false;
        patrolRadius = 5.0f;
        navAgent.speed = walkSpeed; // �ȴ� �ӵ��� ����
    }

    void LookAtPlayer()
    {
        // ��ǥ ���������� ���� ���� ���
        Vector3 direction = navAgent.steeringTarget - transform.position;
        direction.y = 0; // Y�� ȸ���� ����

        // ������ ������ ��� ȸ�� ó��
        if (direction != Vector3.zero)
        {
            // ��ǥ ������ ���� ȸ���ϵ��� ����
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 180�� �ݴ�� ȸ��
            Quaternion reversedRotation = targetRotation * Quaternion.Euler(0, 180f, 0);

            // õõ�� ȸ���ϵ��� Slerp ���
            transform.rotation = Quaternion.Slerp(transform.rotation, reversedRotation, Time.deltaTime * 5f);
        }
    }
    public override void Respawn()
    {
        base.Respawn();
    }
}
