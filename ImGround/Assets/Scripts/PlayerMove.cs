/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerMove : MonoBehaviour
{
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
    bool sleeping = false;

    public bool IsJumping { get { return isJumping; } }
    public bool IsWalking {  get { return isWalking; } }
    public bool IsTired { get { return isTired; } set { isTired = value; } }
    public bool Sleeping { get { return sleeping; } }

    public Player player;
    Animator anim;
    Vector3 moveVec;
    public Rigidbody rigid;
    public Camera followCamera;

    [SerializeField]
    private float runningEnergyTime = 10f;
    private float runningTime = 0f;

    private void Awake()
    {
        if(followCamera == null)
        {
            followCamera = Camera.main;
        }
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
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
        moveVec = (Quaternion.Euler(0.0f, followCamera.transform.rotation.eulerAngles.y, 0.0f) * new Vector3(hAxis, 0.0f, vAxis)).normalized;
        transform.position += moveVec * speed * (rDown ? 1f : 0.5f) * Time.deltaTime;


        isWalking = moveVec != Vector3.zero;
        isRunning = rDown && moveVec != Vector3.zero;

        // 애니메이션 설정
        anim.SetBool("isWalk", isWalking);
        anim.SetBool("isRun", isRunning);

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
    public void Turn()
    {
        //transform.LookAt(transform.position + moveVec);
        if (moveVec != Vector3.zero) // 움직임이 있을 때만 회전
        {
            // 현재 회전과 목표 회전(이동 벡터 방향) 계산
            Quaternion targetRotation = Quaternion.LookRotation(moveVec);

            // 천천히 회전하도록 Slerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
    public void Jump()
    {
        jumpDelay += Time.deltaTime;
        isJumpReady = 1.1f < jumpDelay;

        if (jDown && isJumpReady && !player.pBehavior.IsDigging && !sleeping && !player.pBehavior.IsEating && !player.pBehavior.IsPickingUp)
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
            sleeping = true;
            isTired = true;
            anim.SetBool("isSleep", true);
        }
        if (sleeping && jDown)
        {
            anim.SetBool("isSleep", false);
            StartCoroutine(ResetSleep());
        }
    }
    IEnumerator ResetSleep()
    {
        yield return new WaitForSeconds(2.3f);
        isTired = false;
        sleeping = false;
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
        yield return new WaitForSeconds(1.1f); // 땅파기 애니메이션이 끝나는 시간 (임의로 설정)
        isJumping = false;
    }
}*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
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
    public bool IsWalking { get { return isWalking; } }
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
        // followCamera가 null이 아니면 이동 처리
        if (followCamera != null)
        {
            moveVec = (Quaternion.Euler(0.0f, followCamera.transform.rotation.eulerAngles.y, 0.0f) * new Vector3(hAxis, 0.0f, vAxis)).normalized;
            transform.position += moveVec * speed * (rDown ? 1f : 0.5f) * Time.deltaTime;

            isWalking = moveVec != Vector3.zero;
            isRunning = rDown && moveVec != Vector3.zero;

            // 애니메이션 설정
            anim.SetBool("isWalk", isWalking);
            anim.SetBool("isRun", isRunning);

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

            // 침대를 발견한 경우
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Chair"))  // 침대의 태그가 "Chair"인지 확인
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
                    break;  // 첫 번째 침대에 반응 후 종료
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
}
