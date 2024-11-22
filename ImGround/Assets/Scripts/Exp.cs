using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Exp : MonoBehaviour
{
    public Transform target; // 플레이어 Transform을 할당합니다.
    private Player player;
    public float moveSpeed = 7f; // 경험치 오브젝트의 이동 속도
    public int playerLayer = 7; // 플레이어 레이어 번호 (Player 레이어가 8번이라고 가정)

    void Start()
    {
        // 씬이 로드될 때마다 플레이어를 찾아 타겟을 설정
        FindPlayerTarget();
    }
    private void Update()
    {
        // 플레이어를 향해 이동
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
        // DontDestroy된 플레이어를 찾아 타겟으로 설정
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
        // 플레이어 레이어와 충돌 시 오브젝트 삭제
        if (collision.gameObject.layer == playerLayer)
        {
            // 경험치 획득 처리 (필요시 플레이어의 경험치 증가 코드 작성)
            Destroy(gameObject); // 오브젝트 삭제
        }
    }
}

