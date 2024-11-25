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

        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� ������ ī�޶� ���Ҵ�
        AssignCamera();
    }

    void AssignCamera()
    {
        // ���� �ִ� ���� ī�޶� ã�Ƽ� �Ҵ�
        if (followCamera == null)
        {
            followCamera = Camera.main;
            if (followCamera == null)
            {
                Debug.LogError("���� ī�޶� ã�� �� �����ϴ�. ���� ī�޶� �ִ��� Ȯ���ϼ���.");
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
        // �÷��̾ �ൿ ������ Ȯ��
        bool isPerformingAction = player.pBehavior.dDown || player.pBehavior.IsDigging || isSleeping ||
                                  isSitting || player.pBehavior.IsEating || player.pBehavior.IsPickingUp ||
                                  player.pBehavior.IsHarvest || player.pBehavior.IsPicking;

        // �ൿ ���̸� �̵� �Ұ�
        if (followCamera != null && !isPerformingAction)
        {
            // ī�޶��� ������ �������� �̵� ���� ���
            moveVec = (Quaternion.Euler(0.0f, followCamera.transform.rotation.eulerAngles.y, 0.0f) * new Vector3(hAxis, 0.0f, vAxis)).normalized;
            transform.position += moveVec * speed * (rDown ? 1f : 0.5f) * Time.deltaTime;

            // �̵� �� �޸��� ���� ������Ʈ
            isWalking = moveVec != Vector3.zero;
            isRunning = rDown && moveVec != Vector3.zero;

            // �ִϸ��̼� ����
            anim.SetBool("isWalk", isWalking);
            anim.SetBool("isRun", isRunning);

            // �޸��� �� �Ƿε� ó��
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
            // �ൿ ���̸� �̵� �ִϸ��̼� ��Ȱ��ȭ
            moveVec = Vector3.zero;
            isWalking = false;
            isRunning = false;

            // �ִϸ��̼� ���� �ʱ�ȭ
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }
    }

    public void Turn()
    {
        if (moveVec != Vector3.zero) // �������� ���� ���� ȸ��
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

            // ħ�븦 �߰��� ���
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Bed"))  // ħ���� �±װ� "Bed"���� Ȯ��
                {
                    isSleeping = true;
                    isTired = true;
                    // �ڽ� ������Ʈ�� �ε����� �������� (��: ù ��° �ڽ�)
            Transform childTransform = hitCollider.transform.GetChild(0);  // 0��° �ڽ� ��������

            if (childTransform != null)
            {
                StartCoroutine(Sleeping(childTransform));  // �ڽ� ������Ʈ�� ��ġ�� �̵�
            }
            else
            {
                Debug.LogWarning("�ڽ� ������Ʈ�� ã�� �� �����ϴ�.");
            }
                    break;  // ù ��° ħ�뿡 ���� �� ����
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

            // ���ڸ� �߰��� ���
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Chair"))  // ������ �±װ� "Chair"���� Ȯ��
                {
                    isSitting = true;
                    isTired = true;
                    // �ڽ� ������Ʈ�� �ε����� �������� (��: ù ��° �ڽ�)
                    Transform childTransform = hitCollider.transform.GetChild(0);  // 0��° �ڽ� ��������

                    if (childTransform != null)
                    {
                        StartCoroutine(Sitting(childTransform));  // �ڽ� ������Ʈ�� ��ġ�� �̵�
                    }
                    else
                    {
                        Debug.LogWarning("�ڽ� ������Ʈ�� ã�� �� �����ϴ�.");
                    }
                    break;  // ù ��° ���ڿ� ���� �� ����
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
        // �÷��̾ ħ�� ��ġ�� �̵���Ŵ
        while (Vector3.Distance(transform.position, bedPosition.position) > 0.1f)
        {
            // ħ�뿡 �����ϸ� ħ���� ������ ���� ȸ���ϵ��� ����
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
            // ħ�뿡 �����ϸ� ħ���� ������ ���� ȸ���ϵ��� ����
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
