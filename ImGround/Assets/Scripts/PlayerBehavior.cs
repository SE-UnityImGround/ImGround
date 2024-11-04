using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    Animator anim;
    private Player player;
    public GameObject[] tools;

    bool dDown;
    bool fDown;
    //bool eDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool sDown5;
    bool sDown6;
    bool sDown7;
    bool sDown0;

    bool isDigReady;
    bool isPickReady;
    bool isHarvestReady;
    bool isEating = false;
    bool isPickingUp = false;
    bool isDigging = false;
    bool isPicking = false;
    bool isHarvest = false;
    bool isDie = false;

    public Transform handPoint; // �������� �ݱ� ���� ���� ��ġ
    public Transform pointH;
    private GameObject pickedItem; // ���� �ֿ� ������
    public bool IsEating {  get { return isEating; } }
    public bool IsPickingUp { get {  return isPickingUp; } }
    public bool IsDigging { get {  return isDigging; } }
    public bool IsPicking { get { return isPicking; } }
    public bool IsHarvest { get { return isHarvest; } }
    public int ToolIndex { get { return toolIndex; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }

    float digDelay;
    float pickDelay;
    float harvestDelay;
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
        //eDown = Input.GetKeyDown(KeyCode.E);
        sDown1 = Input.GetKeyDown(KeyCode.Alpha1); // 1�� Ű
        sDown2 = Input.GetKeyDown(KeyCode.Alpha2); // 2�� Ű
        sDown3 = Input.GetKeyDown(KeyCode.Alpha3); // 3�� Ű
        sDown4 = Input.GetKeyDown(KeyCode.Alpha4); // 4�� Ű
        sDown5 = Input.GetKeyDown(KeyCode.Alpha5); // 5�� Ű
        sDown6 = Input.GetKeyDown(KeyCode.Alpha6);
        sDown7 = Input.GetKeyDown(KeyCode.Alpha7);
        sDown0 = Input.GetKeyDown(KeyCode.Alpha0);
    }

    // ���� ��� �� �ൿ ����
    public void Use()
    {
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        harvestDelay = Time.deltaTime;
        isDigReady = 1.5f < digDelay;
        isPickReady = 1.2f < pickDelay;
        isHarvestReady = 0.4f < harvestDelay;

        if(toolIndex == 0 && dDown && !player.pMove.IsJumping && !player.pAttack.IsAttacking)
        {// ���� �Ա�
            isEating = true;
            anim.SetTrigger("doEat");
            StartCoroutine(ResetEat());
        }
        else if (toolIndex == 0 && fDown && !isPickingUp && !isHarvest && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {
            // ���� ������ ������ ���� (OverlapSphere ���)
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.3f); // �÷��̾� �ֺ� 1���� ����

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("fruit") || hitCollider.CompareTag("crop")) // ���� �±װ� �ִ��� Ȯ�� (�ִٸ� pickedItem�� �ش� ������ ����)
                {
                    pickedItem = hitCollider.gameObject;
                    isPickingUp = true;
                    player.pMove.IsTired = true;
                    anim.SetTrigger("doPickUp");

                    // ������ ������ �ݱ� ����
                    StartCoroutine(Picking());

                    // ���� ȿ�� ���� (�������� �տ��� ���ٴ� ���� �ذ�)
                    if (pickedItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        rb.isKinematic = true; // ���� �ùķ��̼� �����Ͽ� ����
                    }

                    // ������ Collider ��Ȱ��ȭ (�÷��̾���� �浹�� �÷��̾ ���ư��� ���� ����)
                    if (pickedItem.TryGetComponent<Collider>(out Collider itemCollider))
                    {
                        itemCollider.enabled = false; // �浹 ��Ȱ��ȭ
                    }

                    // ������ �ݱ� ���� ����
                    StartCoroutine(ResetPickUp());
                    break; // �� �� �������� ������ ���� ����(���� �� �������� ������ ���ÿ� �ݴ� �� ����)
                }
            }
        }
        //else if (toolIndex == 0 && eDown && !isPickingUp && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        //{
        //    // E Ű�� ������ �� �� ���� �õ�
        //    Ray ray = new Ray(transform.position, transform.forward);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 3.0f)) // �÷��̾� �� 3���� �Ÿ� üũ
        //    {
        //        if (hit.collider.CompareTag("Door"))
        //        {
        //            StartCoroutine(OpenAndCloseDoor(hit.collider.gameObject)); // �� ���ݱ� �ִϸ��̼� ����
        //        }
        //    }
        //}
        else if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// �����ϱ�
            anim.SetTrigger("doDigDown");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
        else if (toolIndex == 2 && dDown && isPickReady && !isHarvest && !player.pAttack.IsAttacking)
        {// ���� ��Ȯ
            player.rigid.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            anim.SetTrigger("doPick");
            isPicking = true;
            pickDelay = 0f;
            StartCoroutine(ResetPick());
        }
        else if (toolIndex == 4 && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// ���ı�
            anim.SetTrigger("doDigUp");
            isDigging = true;
            digDelay = 0f;
            StartCoroutine(ResetDig());
        }
        else if(toolIndex == 5 && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {
            anim.SetTrigger("doHarvest");
            isHarvest = true;
            pointH.gameObject.SetActive(true);
            harvestDelay = 0f;
            StartCoroutine(ResetHarvest());
        }
    }
   
    // ������ �ݱ� ���� Ȯ�ο�(���� ���� ����)
    private void OnDrawGizmosSelected()
    {
        if (handPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(handPoint.position, 1f);
    }
    // ��� ����
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
        if (sDown6) // ��
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 5;
        }
        if (sDown7) // ��
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 6;
        }
        if (sDown0) // �̽��Ϳ���
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 7;
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
        yield return new WaitForSeconds(2.4f); // Wait for the pick-up animation to finish
        player.pMove.IsTired = false;
        isPickingUp = false;

        // Destroy the picked-up item
        if (pickedItem != null)
        {
            Destroy(pickedItem);
            pickedItem = null; // Reset the reference to the item
        }
    }

    IEnumerator ResetEat()
    {
        yield return new WaitForSeconds(1.5f);
        isEating = false;
    }

    IEnumerator ResetHarvest()
    {
        yield return new WaitForSeconds(1f);
        pointH.gameObject.SetActive(false);
        isHarvest = false;
    }
    IEnumerator Picking()
    {
        // �������� �� ��ġ�� �̵�
        yield return new WaitForSeconds(0.5f);
        pickedItem.transform.position = handPoint.position;
        pickedItem.transform.rotation = handPoint.rotation; // ���� ȸ���� ����
        pickedItem.transform.parent = handPoint; // �������� �տ� ����
    }
    // ���� ��Ȯ ����
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
                player.health -= 3;
            }
            else if (other.tag == "BossRock")
            {
                anim.SetTrigger("doHit");
                player.health -= 5;
            }
            else if(other.tag == "EnemyAttack")
            {
                anim.SetTrigger("doHit");
                player.health -= 1;
            }
            if (player.health <= 0)
            {
                Die();
            }
        }
    }
}
