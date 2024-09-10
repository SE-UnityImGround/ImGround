using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;

    bool rDown;
    bool fDown;
    bool isReady;
    float attackDelay;

    public Camera followCamera;
    Animator anim;
    Vector3 moveVec;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        getInput();
        Move();
        Turn();
        Attack();
    }

    void getInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        fDown = Input.GetButton("Fire1");
    }
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (rDown ? 1f : 0.3f) * Time.deltaTime;
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Attack()
    {
        attackDelay += Time.deltaTime;
        isReady = 0.4f < attackDelay;
        if(fDown && isReady)
        {
            anim.SetTrigger("doAttack");
            attackDelay = 0f;
        }
    }
}
