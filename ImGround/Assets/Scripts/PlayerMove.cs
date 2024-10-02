using System.Collections;
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
    public bool isJumping = false;
    bool isJumpReady;
    public bool isTired = false;
    public bool sleeping = false;

    private Player player;
    Animator anim;
    Vector3 moveVec;
    public Rigidbody rigid;
    public Camera followCamera;

    [SerializeField]
    private float runningEnergyTime = 10f;
    private float runningTime = 0f;

    private void Awake()
    {
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


        bool isWalking = moveVec != Vector3.zero;
        bool isRunning = rDown && moveVec != Vector3.zero;

        // �ִϸ��̼� ����
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
        transform.LookAt(transform.position + moveVec);
    }
    public void Jump()
    {
        jumpDelay += Time.deltaTime;
        isJumpReady = 1.1f < jumpDelay;

        if (jDown && isJumpReady && !player.pBehavior.isDigging && !sleeping)
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
        yield return new WaitForSeconds(1.1f); // ���ı� �ִϸ��̼��� ������ �ð� (���Ƿ� ����)
        isJumping = false;
    }
}