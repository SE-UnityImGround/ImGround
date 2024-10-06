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



//����-����1
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
        // �÷��̾� �ߺ� ���� �� DontDestroyOnLoad ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �÷��̾ �� ��ȯ �� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �÷��̾� �ı�
            return;
        }

        // �÷��̾� ������Ʈ �ʱ�ȭ
        pMove = GetComponent<PlayerMove>();
        pAttack = GetComponent<PlayerAttack>();
        pBehavior = GetComponent<PlayerBehavior>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �÷��̾� ���� ������Ʈ
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


//����-����2
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
        // �÷��̾� �ߺ� ���� �� DontDestroyOnLoad ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʰ� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �÷��̾� �ı�
            return;
        }

        // �÷��̾� ������Ʈ �ʱ�ȭ
        pMove = GetComponent<PlayerMove>();
        pAttack = GetComponent<PlayerAttack>();
        pBehavior = GetComponent<PlayerBehavior>();
        rigid = GetComponent<Rigidbody>();
    }

    // �̱������� �÷��̾� ������Ʈ ���� ��ȯ
    public static Player GetInstance()
    {
        return instance;
    }

    void Update()
    {
        // �÷��̾� ���� ������Ʈ
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