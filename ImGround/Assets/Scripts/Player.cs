using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public AudioSource[] effectSound;
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
    [SerializeField]
    private Transform[] hat = new Transform[5];

    private static Player instance;
    private bool isDeadCooldown = false; // ��� �� 5�� ������ ��ٿ�
    public int MaxHealth { get { return maxHealth; } }
    public int Exp {  get { return exp; } }
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

        // �÷��̾� ���� ������Ʈ
        pBehavior.getInput();
        pMove.MoveInput();
        pAttack.AttackInput();
        pMove.Sit();
        if (!pMove.IsTired && !pBehavior.IsPickingUp)
        {
            pBehavior.Use();
            pBehavior.Swap();
            pMove.Move();
            pMove.Jump();
            pMove.Sleep();
            pAttack.Attack();
            pAttack.SpinAttack();
        }
        pMove.Turn();
        if (LevelUpCheck())
        {
            exp = 0;
            LevelUp();
        }
        
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
        if (effectSound.Length > 0 && effectSound[0] != null)
        {
            effectSound[0].Play();
        }
        else
        {
            Debug.LogError("ȿ���� �迭�� ����ְų� 0��° �ε����� null�Դϴ�. ȿ������ ����� �� �����ϴ�.");
        }
        // ü���� �ʱ�ȭ
        health = maxHealth;

        // ������ ��ġ�� �̵�
        transform.position = respawnPosition;

        // ��� ���� ����
        pBehavior.IsDie = false;
    }
    private bool LevelUpCheck()
    {
        if (level >= requiredExp.Length)
            return false;
        else if (exp >= requiredExp[level])
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

            if (effectSound.Length > 0 && effectSound[2] != null)
            {
                effectSound[2].Play();
            }
            else
            {
                Debug.LogError("ȿ���� �迭�� ����ְų� 2��° �ε����� null�Դϴ�. ȿ������ ����� �� �����ϴ�.");
            }
        }
        particleSystem?.Play();

        if (hat[level - 1] != null)
        {
            hat[level - 1].gameObject.SetActive(false);
        }
        hat[level].gameObject.SetActive(true);    
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Exp"))
        {
           
            if (effectSound.Length > 0 && effectSound[1] != null)
            {
                effectSound[1].Play();
            }
            else
            {
                Debug.LogError("ȿ���� �迭�� ����ְų� 1��° �ε����� null�Դϴ�. ȿ������ ����� �� �����ϴ�.");
            }
            exp++;
            
        }
    }
}
