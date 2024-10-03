using System.Collections;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    Animator anim;
    private Player player;
    public GameObject[] tools;

    bool dDown;
    bool fDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool sDown5;

    bool isDigReady;
    bool isPickReady;
    bool isEating = false;
    bool isPickingUp = false;
    bool isDigging = false;
    bool isPicking = false;
    bool isDie = false;

    public bool IsEating {  get { return isEating; } }
    public bool IsPickingUp { get {  return isPickingUp; } }
    public bool IsDigging { get {  return isDigging; } }
    public bool IsPicking { get { return isPicking; } }

    float digDelay;
    float pickDelay;
    int toolIndex = 0;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    public void getInput()
    {
        dDown = Input.GetButton("Fire2");
        fDown = Input.GetKeyDown(KeyCode.F);
        sDown1 = Input.GetKeyDown(KeyCode.Alpha1); // 1번 키
        sDown2 = Input.GetKeyDown(KeyCode.Alpha2); // 2번 키
        sDown3 = Input.GetKeyDown(KeyCode.Alpha3); // 3번 키
        sDown4 = Input.GetKeyDown(KeyCode.Alpha4); // 4번 키
        sDown5 = Input.GetKeyDown(KeyCode.Alpha5); // 5번 키
    }

    // 도구 사용 및 행동 로직
    public void Use()
    {
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        isDigReady = 1.5f < digDelay;
        isPickReady = 1.2f < pickDelay;

        if(toolIndex == 0 && dDown && !player.pMove.IsJumping && !player.pAttack.IsAttacking)
        {// 음식 먹기
            isEating = true;
            anim.SetTrigger("doEat");
            StartCoroutine(ResetEat());
        }
        else if(toolIndex == 0 && fDown && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {// 아이템 줍기
            isPickingUp = true;
            player.pMove.IsTired = true;
            anim.SetTrigger("doPickUp");
            StartCoroutine(ResetPickUp());
        }
        else if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// 경작하기
            anim.SetTrigger("doDigDown");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
        else if (toolIndex == 2 && dDown && isPickReady && !player.pAttack.IsAttacking)
        {// 과일 수확
            player.rigid.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            anim.SetTrigger("doPick");
            isPicking = true;
            pickDelay = 0f;
            StartCoroutine(ResetPick());
        }
        else if (toolIndex == 4 && dDown && isDigReady && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// 땅파기
            anim.SetTrigger("doDigUp");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
    }
    // 사망 동작
    public void Die()
    {
        anim.SetTrigger("doDie");
        isDie = true;
    }
    public void Swap()
    {
        int currentIndex = toolIndex;
        if (sDown1) // 주먹
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 0;
        }
        if (sDown2) // 괭이
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 1;
        }
        if (sDown3) // 삼지창(과일 수확용)
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 2;
        }
        if (sDown4) // 곡괭이
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 3;
        }
        if (sDown5) // 삽
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 4;
        }

        tools[toolIndex].gameObject.SetActive(true);
    }
    IEnumerator ResetDig()
    {
        yield return new WaitForSeconds(1.5f); 
        isDigging = false;
    }
    IEnumerator ResetPick()
    {
        yield return new WaitForSeconds(1.2f); 
        isPicking = false;
    }

    IEnumerator ResetPickUp()
    {
        yield return new WaitForSeconds(3.5f);
        player.pMove.IsTired = false;
        isPickingUp = false;
    }

    IEnumerator ResetEat()
    {
        yield return new WaitForSeconds(1.5f);
        isEating = false;
    }
    // 과일 수확 로직
    private void OnTriggerEnter(Collider other)
    {
        if (!isDie)
        {
            if (other.tag == "fruit" && toolIndex == 2 && isPicking)
            {
                Rigidbody fruitRb = other.GetComponent<Rigidbody>();
                Collider fruitCollider = other.GetComponent<Collider>();
                if (fruitRb != null)
                {
                    fruitRb.useGravity = true;
                    fruitCollider.isTrigger = false;
                }
            }
            else if (other.tag == "BossAttack")
            {
                anim.SetTrigger("doHit");
                player.health--;
            }
            else if (other.tag == "BossRock")
            {
                anim.SetTrigger("doHit");
                player.health -= 2;
            }
            if (player.health <= 0)
            {
                Die();
            }
        }
    }
}
