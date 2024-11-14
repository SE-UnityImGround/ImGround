using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class Animal : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private int maxHealth = 5;
    public int health;
    [SerializeField]
    private NavMeshAgent nav;

    public float patrolRadius = 5.0f; // ���� ����
    public float patrolWaitTime = 3.0f; // �� ���� �������� ����ϴ� �ð�
    protected float patrolWaitTimer;

    protected bool surprised = false; // �� ���� �÷���
    protected bool flying = false; // ���߿� ���ƴٴϴ� ����(����) ����
    private bool isDie = false;

    protected NavMeshAgent navAgent;
    private Vector3 patrolTarget;
    public Animator anim;
    public Transform target;

    [Header("Item Reward")]
    public GameObject item;

    void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        SetNewRandomPatrolTarget();
    }

    protected void Update()
    {
        if (!isDie)
        {
            Patrol();
            if (!surprised && !flying)
                LookAt();
        }
    }

    void Patrol()
    {
        // ���� �� �������� �����ߴ��� Ȯ��
        //navAgent.remainingDistance < 0.5f
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            anim.SetBool("isWalk", false);
            patrolWaitTimer += Time.deltaTime;

            // ��� �ð��� ������ ���ο� ���� ������ ����
            if (patrolWaitTimer >= patrolWaitTime)
            {

                SetNewRandomPatrolTarget();
                patrolWaitTimer = 0.0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            StartCoroutine(Die());
    }

    public void StartDie()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        float currentZAngle = transform.eulerAngles.z;
        isDie = true;
        nav.enabled = false;

        Renderer renderer = GetComponentInChildren<Renderer>();
        Color originalColor = renderer.material.color;
        Color targetColor = Color.red;

        float elapsedTime = 0f;
        float duration = 2f; // ������ ���ϴ� �ð� (2�� ����)

        while (elapsedTime < duration)
        {
            // ���������� ���������� ���ϰ� ����
            if (renderer != null) // �������� �����ϴ��� Ȯ��
            {
                renderer.material.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
            }

            // ȸ�� ���� ���������� ����
            float t = elapsedTime / duration;
            float newZAngle = Mathf.Lerp(currentZAngle, 90f, t); // Lerp�� ����� ���������� ȸ��
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZAngle);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // ȸ���� ���������� 90���� ����
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 90f);

        // ���� ���� ���������� ����
        if (renderer != null)
        {
            renderer.material.color = targetColor;
        }

        yield return new WaitForSeconds(3f);
        GameObject reward = Instantiate(item, transform.position, item.transform.rotation);
        Destroy(gameObject);
    }



    // ������ ��ġ�� ���� �������� ����
    protected void SetNewRandomPatrolTarget()
    {

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            patrolTarget = hit.position;
            navAgent.SetDestination(patrolTarget);
        }
        anim.SetBool("isWalk", true);
    }

    void LookAt()
    {
        float targetRadius = 2f;
        float targetRange = 4f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0)
        {
            // Player���� �Ÿ� ���
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            // ���� �Ÿ� �̳��� ���� ��쿡�� �÷��̾ ����
            if (distanceToPlayer <= targetRange)
            {
                navAgent.isStopped = true;
                Vector3 lookDirection = (target.position - transform.position).normalized;
                lookDirection.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ��

                // ��ǥ ȸ�� �� ���
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                // ���� ȸ�� ������ ��ǥ ȸ�� ������ �ε巴�� ȸ�� (Slerp ���)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

                anim.SetBool("isWalk", false);
            }
            else
            {
                // �÷��̾ ������ ����� NavMeshAgent �ٽ� Ȱ��ȭ
                navAgent.isStopped = false;
                anim.SetBool("isWalk", true);
            }
        }
    }

}



/*using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Animal : MonoBehaviour
{
    public float patrolRadius = 5.0f; // ���� ����
    public float patrolWaitTime = 3.0f; // �� ���� �������� ����ϴ� �ð�
    private float patrolWaitTimer;

    protected bool surprised = false; // �� ���� �÷���
    protected bool flying = false; // ���߿� ���ƴٴϴ� ����(����) ����

    protected NavMeshAgent navAgent;
    public Vector3 patrolTarget;
    public Animator anim;
    public Transform target;

    void Awake()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        SetNewRandomPatrolTarget();
    }

    protected void Update()
    {
        Patrol();
        if (!surprised && !flying)
            LookAt();
    }

    void Patrol()
    {
        // ���� �� �������� �����ߴ��� Ȯ��
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            anim.SetBool("isWalk", false);
            patrolWaitTimer += Time.deltaTime;

            // ��� �ð��� ������ ���ο� ���� ������ ����
            if (patrolWaitTimer >= patrolWaitTime)
            {

                SetNewRandomPatrolTarget();
                patrolWaitTimer = 0.0f;
            }
        }
    }

    // ������ ��ġ�� ���� �������� ����
    protected virtual void SetNewRandomPatrolTarget()
    {

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            patrolTarget = hit.position;
            navAgent.SetDestination(patrolTarget);
        }
        anim.SetBool("isWalk", true);
    }

    void LookAt()
    {
        float targetRadius = 2f;
        float targetRange = 4f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0)
        {
            // Player���� �Ÿ� ���
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            // ���� �Ÿ� �̳��� ���� ��쿡�� �÷��̾ ����
            if (distanceToPlayer <= targetRange)
            {
                navAgent.isStopped = true;
                Vector3 lookDirection = (target.position - transform.position).normalized;
                lookDirection.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ��

                // ��ǥ ȸ�� �� ���
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                // ���� ȸ�� ������ ��ǥ ȸ�� ������ �ε巴�� ȸ�� (Slerp ���)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

                anim.SetBool("isWalk", false);
            }
            else
            {
                // �÷��̾ ������ ����� NavMeshAgent �ٽ� Ȱ��ȭ
                navAgent.isStopped = false;
                anim.SetBool("isWalk", true);
            }
        }
    }

}
*/
