using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class PlayerMove : MonoBehaviour
{
    public AudioSource[] effectSound;
    public float speed;
    float hAxis;
    float vAxis;
    float jumpDelay;

    bool rDown;
    bool jDown;
    bool isJumping = false;
    bool isJumpReady;
    bool isWalking;
    bool isRunning;
    bool isTired = false;
    bool isSleeping = false;
    bool isSitting = false;

    public bool IsJumping { get { return isJumping; } }
    public bool IsWalking { get { return isWalking; } set { isWalking = value; } }
    public bool IsRunning { get { return isRunning; } set { isRunning = value; } }
    public bool IsTired { get { return isTired; } set { isTired = value; } }
    public bool IsSleeping { get { return isSleeping; } }
    public bool IsSitting { get { return isSitting; } }
    public Player player;
    Animator anim;
    Vector3 moveVec;
    public Rigidbody rigid;
    public Camera followCamera;

    [SerializeField]
    private float runningEnergyTime = 10f;
    private float runningTime = 0f;

    [SerializeField]
    private GameObject particle;
    private GameObject particleInstance;
    private ParticleSystem particleSystem;

    private void Awake()
    {
        AssignCamera();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        player = GetComponent<Player>();

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때마다 카메라 재할당
        AssignCamera();
    }

    void AssignCamera()
    {
        // 씬에 있는 메인 카메라를 찾아서 할당
        if (followCamera == null)
        {
            followCamera = Camera.main;
            if (followCamera == null)
            {
                Debug.LogError("메인 카메라를 찾을 수 없습니다. 씬에 카메라가 있는지 확인하세요.");
            }
        }
    }

    public void MoveInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetKeyDown(KeyCode.Space);
    }

    public void Move()
    {
        // 플레이어가 행동 중인지 확인
        bool isPerformingAction = player.pBehavior.dDown || player.pBehavior.IsDigging || isSleeping ||
                                  isSitting || player.pBehavior.IsEating || player.pBehavior.IsPickingUp ||
                                  player.pBehavior.IsHarvest || player.pBehavior.IsPicking;

        // 행동 중이면 이동 불가
        if (followCamera != null && !isPerformingAction)
        {
            // 카메라의 방향을 기준으로 이동 벡터 계산
            moveVec = (Quaternion.Euler(0.0f, followCamera.transform.rotation.eulerAngles.y, 0.0f) * new Vector3(hAxis, 0.0f, vAxis)).normalized;
            transform.position += moveVec * speed * (rDown ? 1f : 0.5f) * Time.deltaTime;

            // 이동 및 달리기 상태 업데이트
            isWalking = moveVec != Vector3.zero;
            isRunning = rDown && moveVec != Vector3.zero;

            // 애니메이션 설정
            anim.SetBool("isWalk", isWalking);
            anim.SetBool("isRun", isRunning);

            // 달리는 중 피로도 처리
            if (isRunning)
            {
                runningTime += Time.deltaTime;
                if (runningTime > runningEnergyTime)
                {
                    StartCoroutine(Tired());
                }
            }
            else
            {
                runningTime = 0;
            }
        }
        else
        {
            // 행동 중이면 이동 애니메이션 비활성화
            moveVec = Vector3.zero;
            isWalking = false;
            isRunning = false;

            // 애니메이션 상태 초기화
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }
    }

    public void Turn()
    {
        if (moveVec != Vector3.zero) // 움직임이 있을 때만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVec);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public void Jump()
    {
        jumpDelay += Time.deltaTime;
        isJumpReady = 1.1f < jumpDelay;

        if (jDown && isJumpReady && !player.pBehavior.IsDigging && !isSleeping && !isSitting && !player.pBehavior.IsEating && !player.pBehavior.IsPickingUp)
        {
            isJumping = true;
            rigid.AddForce(Vector3.up * 4.5f, ForceMode.Impulse);
            anim.SetTrigger("doJump");
            jumpDelay = 0f;
            StartCoroutine(ResetJump());
        }
    }

    public void Sleep()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);

            // 침대를 발견한 경우
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Bed"))  // 침대의 태그가 "Bed"인지 확인
                {
                    isSleeping = true;
                    isTired = true;
                    // 자식 오브젝트의 인덱스로 가져오기 (예: 첫 번째 자식)
            Transform childTransform = hitCollider.transform.GetChild(0);  // 0번째 자식 가져오기

            if (childTransform != null)
            {
                StartCoroutine(Sleeping(childTransform));  // 자식 오브젝트의 위치로 이동
            }
            else
            {
                Debug.LogWarning("자식 오브젝트를 찾을 수 없습니다.");
            }
                    break;  // 첫 번째 침대에 반응 후 종료
                }
            }
        }
        if (isSleeping && jDown)
        {
            anim.SetBool("isSleep", false);
            StopAllCoroutines();
            StartCoroutine(ResetSleep());
        }
    }
    public void Sit()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);

            // 의자를 발견한 경우
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Chair"))  // 의자의 태그가 "Chair"인지 확인
                {
                    isSitting = true;
                    isTired = true;
                    // 자식 오브젝트의 인덱스로 가져오기 (예: 첫 번째 자식)
                    Transform childTransform = hitCollider.transform.GetChild(0);  // 0번째 자식 가져오기

                    if (childTransform != null)
                    {
                        StartCoroutine(Sitting(childTransform));  // 자식 오브젝트의 위치로 이동
                    }
                    else
                    {
                        Debug.LogWarning("자식 오브젝트를 찾을 수 없습니다.");
                    }
                    break;  // 첫 번째 의자에 반응 후 종료
                }
            }
        }
        if (isSitting && jDown)
        {
            anim.SetBool("isSit", false);
            StopAllCoroutines();
            StartCoroutine(ResetSit());
        }
    }
    IEnumerator Sleeping(Transform bedPosition)
    {
        anim.SetBool("isSleep", true);
        // 플레이어를 침대 위치로 이동시킴
        while (Vector3.Distance(transform.position, bedPosition.position) > 0.1f)
        {
            // 침대에 도착하면 침대의 방향을 따라 회전하도록 설정
            transform.rotation = Quaternion.Euler(0, bedPosition.eulerAngles.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, bedPosition.position, 5f * Time.deltaTime);
            yield return null;
        }
        if (particleInstance == null)
        {
            particleInstance = Instantiate(particle, transform.position, Quaternion.Euler(-90, 0, 0));
            particleSystem = particleInstance.GetComponent<ParticleSystem>();
        }
        particleSystem?.Play();
        yield return new WaitForSeconds(1f);
        while(player.health < player.MaxHealth)
        {
            player.health += 3;
            yield return new WaitForSeconds(1.2f);
        }
    }

    IEnumerator Sitting(Transform chairPos)
    {
        anim.SetBool("isSit", true);
        while (Vector3.Distance(transform.position, chairPos.position) > 0.1f)
        {
            // 침대에 도착하면 침대의 방향을 따라 회전하도록 설정
            transform.rotation = Quaternion.Euler(0, chairPos.eulerAngles.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, chairPos.position, 5f * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator ResetSleep()
    {
        yield return new WaitForSeconds(2.3f);
        particleSystem?.Stop();
        Destroy(particleInstance);
        isTired = false;
        isSleeping = false;
    }
    IEnumerator ResetSit()
    {
        yield return new WaitForSeconds(2.3f);
        isTired = false;
        isSitting = false;
    }
    IEnumerator Tired()
    {
        isTired = true;
        anim.SetBool("isTired", true);
        yield return new WaitForSeconds(3f);
        anim.SetBool("isTired", false);
        isTired = false;
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(1.1f);
        isJumping = false;
    }

    private void FixedUpdate()
    {
        if (isWalking && !effectSound[0].isPlaying)
        {
            effectSound[0].Play();
        }
        else if((!isWalking && effectSound[0].isPlaying ) || isTired)
        {
            effectSound[0].Stop();
        }
        if (isRunning && !effectSound[1].isPlaying)
        {
            effectSound[1].Play();
        }
        else if ((!isRunning && effectSound[1].isPlaying) || isTired)
        {
            effectSound[1].Stop();
        }
        if (isJumping && !effectSound[2].isPlaying)
        {
            effectSound[2].Play();
        }
        else if ((!isJumping && effectSound[2].isPlaying) && isTired)
        {
            effectSound[2].Stop();
        }
        if (isTired && !effectSound[3].isPlaying)
        {
            effectSound[3].Play();
        }
        else if ((!isTired && effectSound[3].isPlaying))
        {
            effectSound[3].Stop();
        }
    }
}
