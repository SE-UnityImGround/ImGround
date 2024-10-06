/*using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Class")]
    public PlayerMove pMove;
    public PlayerAttack pAttack;
    public PlayerBehavior pBehavior;

    [Header("Player Status")]
    public int health = 5;

    public Rigidbody rigid;

    void Awake()
    {
        pMove = GetComponent<PlayerMove>();
        pAttack = GetComponent<PlayerAttack>();
        pBehavior = GetComponent<PlayerBehavior>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        pBehavior.getInput();
        pBehavior.Use();
        pBehavior.Swap();
        pMove.MoveInput();
        pAttack.AttackInput();
        if (!pMove.IsTired)
        {
            pMove.Move();
        }
        pMove.Turn();
        pMove.Jump();
        pMove.Sleep();
        pAttack.Attack();
    }
}*/



//유진-수정1
/*using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Class")]
    public PlayerMove pMove;
    public PlayerAttack pAttack;
    public PlayerBehavior pBehavior;

    [Header("Player Status")]
    public int health = 5;

    public Rigidbody rigid;

    private static Player instance;

    void Awake()
    {
        // 플레이어 중복 방지 및 DontDestroyOnLoad 적용
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 플레이어를 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 플레이어 파괴
            return;
        }

        // 플레이어 컴포넌트 초기화
        pMove = GetComponent<PlayerMove>();
        pAttack = GetComponent<PlayerAttack>();
        pBehavior = GetComponent<PlayerBehavior>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 플레이어 동작 업데이트
        pBehavior.getInput();
        pBehavior.Use();
        pBehavior.Swap();
        pMove.MoveInput();
        pAttack.AttackInput();

        if (!pMove.IsTired)
        {
            pMove.Move();
        }

        pMove.Turn();
        pMove.Jump();
        pMove.Sleep();
        pAttack.Attack();
    }
}
*/


//유진-수정2
/*using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Class")]
    public PlayerMove pMove;
    public PlayerAttack pAttack;
    public PlayerBehavior pBehavior;

    [Header("Player Status")]
    public int health = 5;

    public Rigidbody rigid;

    private static Player instance;

    void Awake()
    {
        // 플레이어 중복 방지 및 DontDestroyOnLoad 적용
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 플레이어 파괴
            return;
        }

        // 플레이어 컴포넌트 초기화
        pMove = GetComponent<PlayerMove>();
        pAttack = GetComponent<PlayerAttack>();
        pBehavior = GetComponent<PlayerBehavior>();
        rigid = GetComponent<Rigidbody>();
    }

    // 싱글톤으로 플레이어 오브젝트 참조 반환
    public static Player GetInstance()
    {
        return instance;
    }

    void Update()
    {
        // 플레이어 동작 업데이트
        pBehavior.getInput();
        pBehavior.Use();
        pBehavior.Swap();
        pMove.MoveInput();
        pAttack.AttackInput();

        if (!pMove.IsTired)
        {
            pMove.Move();
        }

        pMove.Turn();
        pMove.Jump();
        pMove.Sleep();
        pAttack.Attack();
    }
}
*/