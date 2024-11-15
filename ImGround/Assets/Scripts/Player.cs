using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Player Class")]
    public PlayerMove pMove;
    public PlayerAttack pAttack;
    public PlayerBehavior pBehavior;

    [Header("Player Status")]
    [SerializeField]
    private int maxHealth = 30;
    public int health;
    private int exp;
    public int[] requiredExp = new int[10];
    public int level = 0;

    [Header("Effect Info")]
    [SerializeField]
    private GameObject effect;
    private GameObject effectInstance;
    private ParticleSystem particleSystem;

    public Vector3 respawnPosition; // ������ ��ġ ����
    public Rigidbody rigid;

    private static Player instance;
    private bool isDeadCooldown = false; // ��� �� 5�� ������ ��ٿ�

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
        health = maxHealth;
        // ������ �� �÷��̾��� �⺻ ������ ��ġ�� ���� ��ġ�� ����(ħ�� �߰��� �� �ڵ�� ���� ����)
        respawnPosition = transform.position;
    }

    // �̱������� �÷��̾� ������Ʈ ���� ��ȯ
    public static Player GetInstance()
    {
        return instance;
    }

    void Update()
    {
        // ��� �� ��ٿ��� Ȱ��ȭ�� ���¿����� �ƹ� ���۵� ���� ����
        if (isDeadCooldown)
        {
            return;
        }

        // �÷��̾ ��� �������� ���� Ȯ���ϰ� ��� ó�� �� 5�ʰ� ���� ����
        if (pBehavior.IsDie)
        {
            StartCoroutine(DeathCooldown());
            return;
        }

        if (LevelUpCheck() && level < requiredExp.Length)
        {
            LevelUp();
        }
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
        pMove.Sit();
        pAttack.Attack();
    }
    // ��� �� 5�� ���� ������ �����ϴ� �ڷ�ƾ
    IEnumerator DeathCooldown()
    {
        isDeadCooldown = true; // ��ٿ� ����
        yield return new WaitForSeconds(5f); // 5�� ���

        Respawn();
        isDeadCooldown = false; // ��ٿ� ����
    }
    private void Respawn()
    {
        // ü���� �ʱ�ȭ
        health = maxHealth;

        // ������ ��ġ�� �̵�
        transform.position = respawnPosition;

        // ��� ���� ����
        pBehavior.IsDie = false;
    }
    private bool LevelUpCheck()
    {
        if (exp >= requiredExp[level])
        {
            level++;
            return true;
        }
        else
            return false;
    }
    private void LevelUp()
    {
        if (effectInstance == null)
        {
            // y�� ��ġ�� 1.0f��ŭ �÷��� ��ƼŬ�� ���� (�ʿ信 ���� ���� ���� ����)
            Vector3 adjustedPos = transform.position + new Vector3(0, 2.0f, 0);

            effectInstance = Instantiate(effect, adjustedPos, Quaternion.identity);
            particleSystem = effectInstance.GetComponent<ParticleSystem>();
        }
        particleSystem?.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Exp"))
        {
            exp++;
        }
    }
}
