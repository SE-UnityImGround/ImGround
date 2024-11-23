using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    Animator anim;
    private Player player;
    public GameObject[] tools;

    public bool dDown;
    bool fDown;
    bool eDown;
    bool[] sDown; // 0~7�������� ���� �ε��� ��ȣ ����

    bool isDigReady, isPickReady, isHarvestReady, isPlantReady;
    bool isEating = false;
    bool isPickingUp = false;
    bool isDigging = false;
    bool isPicking = false;
    bool isHarvest = false;
    bool isPlant = false;
    bool isDie = false;

    public Transform handPoint; // �������� �ݱ� ���� ���� ��ġ
    public Transform pickPoint; // �������� �ݱ� ���� ���� ��ġ
    public Transform pointH;  // ���� �ݶ��̴� ��ġ
    public Transform[] curtivatePoint; // ���̿� ���� �ݶ��̴� (0�� �ε��� : ����, 1�� �ε��� : ��)
    public Transform ItemPoint; // ������ �Դ� ���� ��ġ
    private GameObject pickedItem; // ���� �ֿ� ������
    public bool IsEating {  get { return isEating; } }
    public bool IsPickingUp { get {  return isPickingUp; } }
    public bool IsDigging { get {  return isDigging; } }
    public bool IsPicking { get { return isPicking; } }
    public bool IsHarvest { get { return isHarvest; } }
    public int ToolIndex { get { return toolIndex; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }

    float digDelay, pickDelay, harvestDelay, plantDelay;

    int toolIndex = 0; // ���� �÷��̾ �տ� �� ������ �ε���

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        sDown = new bool[8];
    }
    public void getInput()
    {
        dDown = Input.GetButton("Fire2"); // ���� ���� Ű
        fDown = Input.GetKeyDown(KeyCode.F); // �ݱ� Ű
        eDown = Input.GetKeyDown(KeyCode.E);
        sDown[1] = Input.GetKeyDown(KeyCode.Alpha1); // 1�� Ű
        sDown[2] = Input.GetKeyDown(KeyCode.Alpha2); // 2�� Ű
        sDown[3] = Input.GetKeyDown(KeyCode.Alpha3); // 3�� Ű
        sDown[4] = Input.GetKeyDown(KeyCode.Alpha4); // 4�� Ű
        sDown[5] = Input.GetKeyDown(KeyCode.Alpha5); // 5�� Ű
        sDown[6] = Input.GetKeyDown(KeyCode.Alpha6); // 6�� Ű
        sDown[7] = Input.GetKeyDown(KeyCode.Alpha7); // 7�� Ű
        sDown[0] = Input.GetKeyDown(KeyCode.Alpha0); // 0�� Ű
    }

    // ���� ��� �� �ൿ ����
    public void Use()
    {
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        harvestDelay += Time.deltaTime;
        plantDelay += Time.deltaTime;

        isDigReady = 1.8f < digDelay;
        isPickReady = 1.2f < pickDelay;
        isHarvestReady = 0.4f < harvestDelay;
        isPlantReady = 2f < plantDelay;

        if(toolIndex == 0 && dDown && !player.pMove.IsJumping && !player.pAttack.IsAttacking)
        {// ���� �Ա�
            if (ItemPoint.childCount == 0)
                return;
            isEating = true;
            anim.SetTrigger("doEat");
            StartCoroutine(ResetEat());
        }
        else if (toolIndex == 0 && fDown && !isPickingUp && !isDigging && !isHarvest && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {
            // ���� ������ ������ ���� (OverlapSphere ���)
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.3f); // �÷��̾� �ֺ� 1���� ����

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("fruit") || hitCollider.CompareTag("crop") || hitCollider.CompareTag("item")) // Ư�� �±װ� �ִ��� Ȯ�� (�ִٸ� pickedItem�� �ش� ������ ����)
                {
                    pickedItem = hitCollider.gameObject;
                    isPickingUp = true;
                    //player.pMove.IsTired = true;
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
        else if (toolIndex == 0 && eDown && isPlantReady && !isPickingUp && !isHarvest && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {// ���� �ɱ�
            anim.SetTrigger("doPlant");
            isPlant = true;
            plantDelay = 0f;
            StartCoroutine(ResetPlant());
        }
        else if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// �����ϱ� + ä���ϱ�
            anim.SetTrigger("doDigDown");
            isDigging = true;
            digDelay = 0f;
            if(toolIndex == 1)
            {
                curtivatePoint[0].gameObject.SetActive(true);
            }
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
            curtivatePoint[1].gameObject.SetActive(true);
            StartCoroutine(ResetDigUp());
        }
        else if(toolIndex == 5 && dDown && isHarvestReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// �۹� ��Ȯ�ϱ�
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
        if (!isEating && !isDigging && !isPicking && !isPickingUp && !isHarvest) {
            int currentIndex = toolIndex;
            if (sDown[1]) // �ָ�
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 0;
            }
            if (sDown[2]) // ����
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 1;
            }
            if (sDown[3]) // ����â(���� ��Ȯ��)
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 2;
            }
            if (sDown[4]) // ���
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 3;
            }
            if (sDown[5]) // ��
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 4;
            }
            if (sDown[6]) // ��
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 5;
            }
            if (sDown[7]) // ��
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 6;
            }
            if (sDown[0]) // �̽��Ϳ���
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 7;
            }

            tools[toolIndex].gameObject.SetActive(true);
        }
    }
    IEnumerator ResetDig()
    {
        yield return new WaitForSeconds(1.6f);
        isDigging = false;
        curtivatePoint[0].gameObject.SetActive(false);
    }

    IEnumerator ResetDigUp()
    {
        yield return new WaitForSeconds(1.5f);
        isDigging = false;
        curtivatePoint[1].gameObject.SetActive(false);
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
            getItem(pickedItem.GetComponent<ItemPrefabID>()); // �ֿ� �������� �κ��丮 ó��
            pickedItem.transform.SetParent(null);
            pickedItem.gameObject.SetActive(false);
            //Destroy(pickedItem);
            pickedItem = null; // Reset the reference to the item
        }
    }

    private void getItem(ItemPrefabID itemPrefabId)
    {
        if (itemPrefabId == null)
        {
            Debug.LogError("�������� �߰��� �� ���� : ������ ������/������Ʈ�� " + nameof(ItemPrefabID) + " ��ũ��Ʈ�� �����ϴ�!");
        }
        else
        {
            ItemBundle takenItem = itemPrefabId.getItem();
            InventoryManager.addItems(takenItem);
            if (takenItem.count > 0)
            {
                ItemThrowManager.throwItem(takenItem);
            }
        }
    }

    IEnumerator ResetEat()
    {
        yield return new WaitForSeconds(1.5f);
        isEating = false;
    }

    IEnumerator ResetHarvest()
    {
        yield return new WaitForSeconds(0.7f);
        pointH.gameObject.SetActive(false);
        isHarvest = false;
    }

    IEnumerator ResetPlant()
    {
        yield return new WaitForSeconds(2f);
        isPlant = false;
    }
    IEnumerator Picking()
    {
        // �������� �� ��ġ�� �̵�
        yield return new WaitForSeconds(0.5f);
        // ������ ���� ������ ����
        Vector3 originalScale = pickedItem.transform.lossyScale;

        // �θ� ����
        pickedItem.transform.SetParent(handPoint, worldPositionStays: false);

        // �������� ���� ���� �����Ϸ� ����
        pickedItem.transform.localScale = new Vector3(
            originalScale.x / handPoint.lossyScale.x,
            originalScale.y / handPoint.lossyScale.y,
            originalScale.z / handPoint.lossyScale.z
        );
        pickedItem.transform.localPosition = Vector3.zero;
        pickedItem.transform.localRotation = Quaternion.identity; // ���� ȸ���� ����  

        Debug.Log(pickedItem.transform.position);
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
                player.health -= 6;
            }
            else if (other.tag == "BossRock")
            {
                anim.SetTrigger("doHit");
                player.health -= 10;
            }
            else if(other.tag == "EnemyAttack")
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
