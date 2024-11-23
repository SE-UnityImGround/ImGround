using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Exp : MonoBehaviour
{
    public Transform target; // �÷��̾� Transform�� �Ҵ��մϴ�.
    private Player player;
    public float moveSpeed = 7f; // ����ġ ������Ʈ�� �̵� �ӵ�
    public int playerLayer = 7; // �÷��̾� ���̾� ��ȣ (Player ���̾ 8���̶�� ����)

    void Start()
    {
        // ���� �ε�� ������ �÷��̾ ã�� Ÿ���� ����
        FindPlayerTarget();
    }
    private void Update()
    {
        // �÷��̾ ���� �̵�
        if (target != null)
        {
            StartCoroutine(MoveToTarget(target));
        }
    }
    IEnumerator MoveToTarget(Transform target)
    {
        yield return new WaitForSeconds(2f);
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
    void FindPlayerTarget()
    {
        // DontDestroy�� �÷��̾ ã�� Ÿ������ ����
        player = Player.GetInstance();
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾� ���̾�� �浹 �� ������Ʈ ����
        if (collision.gameObject.layer == playerLayer)
        {
            // ����ġ ȹ�� ó�� (�ʿ�� �÷��̾��� ����ġ ���� �ڵ� �ۼ�)
            Destroy(gameObject); // ������Ʈ ����
        }
    }
}

