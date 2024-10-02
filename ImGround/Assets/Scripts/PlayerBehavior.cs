using System.Collections;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    Animator anim;
    private Player player;
    public GameObject[] tools;

    bool dDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool sDown5;

    bool isDigReady;
    bool isPickReady;
    public bool isDigging = false;
    public bool isPicking = false;
    bool isDie = false;

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
        sDown1 = Input.GetKeyDown(KeyCode.Alpha1); // 1�� Ű
        sDown2 = Input.GetKeyDown(KeyCode.Alpha2); // 2�� Ű
        sDown3 = Input.GetKeyDown(KeyCode.Alpha3); // 3�� Ű
        sDown4 = Input.GetKeyDown(KeyCode.Alpha4); // 4�� Ű
        sDown5 = Input.GetKeyDown(KeyCode.Alpha5); // 5�� Ű
    }

    // ���� ���
    public void Use()
    {
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        isDigReady = 1.5f < digDelay;
        isPickReady = 1.2f < pickDelay;

        if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !player.pAttack.isAttacking && !player.pMove.isJumping && !isPicking)
        {
            anim.SetTrigger("doDigDown");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
        else if (toolIndex == 2 && dDown && isPickReady && !player.pAttack.isAttacking)
        {
            player.rigid.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            anim.SetTrigger("doPick");
            isPicking = true;
            pickDelay = 0f;
            StartCoroutine(ResetPick());
        }
        else if (toolIndex == 4 && dDown && isDigReady && !player.pAttack.isAttacking && !player.pMove.isJumping && !isPicking)
        {
            anim.SetTrigger("doDigUp");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
    }
    public void Die()
    {
        anim.SetTrigger("doDie");
        isDie = true;
    }
    public void Swap()
    {
        int currentIndex = toolIndex;
        if (sDown1) // �ָ�
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 0;
        }
        if (sDown2) // ����
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 1;
        }
        if (sDown3) // ����â(���� ��Ȯ��)
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 2;
        }
        if (sDown4) // ���
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 3;
        }
        if (sDown5) // ��
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 4;
        }

        tools[toolIndex].gameObject.SetActive(true);
    }
    IEnumerator ResetDig()
    {
        yield return new WaitForSeconds(1.5f); // ���ı� �ִϸ��̼��� ������ �ð� (���Ƿ� ����)
        isDigging = false;
    }
    IEnumerator ResetPick()
    {
        yield return new WaitForSeconds(1.2f); // ���ı� �ִϸ��̼��� ������ �ð� (���Ƿ� ����)
        isPicking = false;
    }

    // ���� ����
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
