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

    public Vector3 respawnPosition; // 리스폰 위치 설정
    public Rigidbody rigid;
    [SerializeField]
    private Transform[] hat = new Transform[5];

    private static Player instance;
    private bool isDeadCooldown = false; // 사망 후 5초 동안의 쿨다운
    public int MaxHealth { get { return maxHealth; } }
    public int Exp {  get { return exp; } }
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
        health = maxHealth;
        // 시작할 때 플레이어의 기본 리스폰 위치를 현재 위치로 설정(침대 추가시 이 코드는 삭제 예정)
        respawnPosition = transform.position;
    }

    // 싱글톤으로 플레이어 오브젝트 참조 반환
    public static Player GetInstance()
    {
        return instance;
    }

    void Update()
    {
        // 사망 후 쿨다운이 활성화된 상태에서는 아무 동작도 하지 않음
        if (isDeadCooldown)
        {
            return;
        }

        // 플레이어가 사망 상태인지 먼저 확인하고 사망 처리 후 5초간 동작 제한
        if (pBehavior.IsDie)
        {
            StartCoroutine(DeathCooldown());
            return;
        }

        // 플레이어 동작 업데이트
        pBehavior.GetInput();
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
    // 사망 후 5초 동안 동작을 제한하는 코루틴
    IEnumerator DeathCooldown()
    {
        isDeadCooldown = true; // 쿨다운 시작
        yield return new WaitForSeconds(5f); // 5초 대기

        Respawn();
        isDeadCooldown = false; // 쿨다운 종료
    }
    private void Respawn()
    {
        if (effectSound.Length > 0 && effectSound[0] != null)
        {
            effectSound[0].Play();
        }
        else
        {
            Debug.LogError("효과음 배열이 비어있거나 0번째 인덱스가 null입니다. 효과음을 재생할 수 없습니다.");
        }
        // 체력을 초기화
        health = maxHealth;

        // 리스폰 위치로 이동
        transform.position = respawnPosition;

        // 사망 상태 해제
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
            // y축 위치를 1.0f만큼 올려서 파티클을 생성 (필요에 따라 높이 조정 가능)
            Vector3 adjustedPos = transform.position + new Vector3(0, 2.0f, 0);

            effectInstance = Instantiate(effect, adjustedPos, Quaternion.identity);
            particleSystem = effectInstance.GetComponent<ParticleSystem>();

            if (effectSound.Length > 0 && effectSound[2] != null)
            {
                effectSound[2].Play();
            }
            else
            {
                Debug.LogError("효과음 배열이 비어있거나 2번째 인덱스가 null입니다. 효과음을 재생할 수 없습니다.");
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
                Debug.LogError("효과음 배열이 비어있거나 1번째 인덱스가 null입니다. 효과음을 재생할 수 없습니다.");
            }
            exp++;
            
        }
    }
}
