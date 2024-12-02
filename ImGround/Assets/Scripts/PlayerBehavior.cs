using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    public AudioSource[] effectSound;
    Animator anim;
    private Player player;
    private Crops crop;
    public GameObject[] tools;
    private GameObject grabbedItem = null;
    private int grabbingSlotIdx = -1;

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
    bool isGrabbing = false;
    bool canFarming = false;
    bool canPlant = false;

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
    public bool IsPlant {  get { return isPlant; } }
    public int ToolIndex { get { return toolIndex; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }

    float digDelay, pickDelay, harvestDelay, plantDelay;

    int toolIndex = 0; // ���� �÷��̾ �տ� �� ������ �ε���

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        sDown = new bool[8];

        InventoryManager.onSelectionChangedHandler += OnItemSelectionChanged;
    }
    public void GetInput()
    {
        dDown = InputManager.GetButton("Fire2"); // ���� ���� Ű
        fDown = InputManager.GetKeyDown(KeyCode.F); // �ݱ� Ű
        eDown = InputManager.GetKeyDown(KeyCode.E);
        sDown[1] = InputManager.GetKeyDown(KeyCode.Alpha1); // 1�� Ű
        sDown[2] = InputManager.GetKeyDown(KeyCode.Alpha2); // 2�� Ű
        sDown[3] = InputManager.GetKeyDown(KeyCode.Alpha3); // 3�� Ű
        sDown[4] = InputManager.GetKeyDown(KeyCode.Alpha4); // 4�� Ű
        sDown[5] = InputManager.GetKeyDown(KeyCode.Alpha5); // 5�� Ű
        sDown[6] = InputManager.GetKeyDown(KeyCode.Alpha6); // 6�� Ű
        sDown[7] = InputManager.GetKeyDown(KeyCode.Alpha7); // 7�� Ű
        sDown[0] = InputManager.GetKeyDown(KeyCode.Alpha0); // 0�� Ű
    }

    // ���� ��� �� �ൿ ����
    public void Use()
    {
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        harvestDelay += Time.deltaTime;
        plantDelay += Time.deltaTime;

        isDigReady = 1.5f < digDelay;
        isPickReady = 0.8f < pickDelay;
        isHarvestReady = 0.4f < harvestDelay;
        isPlantReady = 2f < plantDelay;

        if(toolIndex == 0 && dDown && !player.pMove.IsJumping && !player.pAttack.IsAttacking)
        {// ���� �Ա�
            if (ItemPoint.childCount == 0
                || IsEating // �Դ� ���� �� �Դ� ������ �����ϴ� ���� ����
                || !isGrabbing // �ƹ� �����۵� ��� ���� �ʴٸ� ó������ ����
                || !ItemInfoManager.getItemInfo(grabbedItem.GetComponent<ItemPrefabID>().itemType).isFood) // ������ ��� �ִ°� �ƴϸ� ó������ ����
                return;

            // ü�� ȸ�� ó��
            float healAmount = ItemInfoManager.getItemInfo(grabbedItem.GetComponent<ItemPrefabID>().itemType).healAmount;
            player.health = Mathf.Min(player.health + (int)(healAmount * player.MaxHealth), player.MaxHealth);

            isEating = true;
            anim.SetTrigger("doEat");
            StartCoroutine(ResetEat());
            effectSound[4].Play();
        }
        else if (toolIndex == 0 && fDown && !isPickingUp && !isDigging && !isHarvest && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking && !isGrabbing)
        {
            Collider nearestCollider = null;
            float nearestDistance = Mathf.Infinity;         
            // ���� ������ ������ ���� (OverlapSphere ���)
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // �÷��̾� �ֺ� 1���� ����

            foreach (Collider hitCollider in hitColliders)
            {
                // Ư�� �±װ� �ִ��� Ȯ��(�ִٸ� pickedItem�� �ش� ������ ����)
                if (hitCollider.CompareTag("fruit") || hitCollider.CompareTag("crop") || hitCollider.CompareTag("item"))
                {
                    // �� �ݶ��̴����� �Ÿ� ���
                    float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestCollider = hitCollider;
                    }
                }
            }
            // �÷��̾�� ���� ������ �ִ� ��ü�� �ݱ�
            pickedItem = nearestCollider.gameObject;
            isPickingUp = true;
            anim.SetTrigger("doPickUp");

            // ������ ������ �ݱ� ����
            StartCoroutine(Picking());
            effectSound[5].Play();

            // ���� ȿ�� ���� (�������� �տ��� ���ٴ� ���� �ذ�)
            if (pickedItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true; // ���� �ùķ��̼� �����Ͽ� ����
            }

            // ������ Collider ��Ȱ��ȭ (�÷��̾���� �浹�� �÷��̾ ���ư��� ���� ����)
            if (pickedItem.TryGetComponent<Collider>(out Collider itemCollider) && !pickedItem.CompareTag("crop"))
            {
                itemCollider.enabled = false; // �浹 ��Ȱ��ȭ
            }

            // ������ �ݱ� ���� ����
            StartCoroutine(ResetPickUp());
        }
        else if (toolIndex == 0 && ItemPoint.childCount > 0 && eDown && canFarming && canPlant && isPlantReady && !isPickingUp && !isHarvest &&
                !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {// ���� �ɱ�
            StartCoroutine(Planting());
            anim.SetTrigger("doPlant");
            isPlant = true;
            plantDelay = 0f;
            effectSound[6].Play();
            StartCoroutine(ResetPlant());
        }

        else if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {
            if (toolIndex == 1 && canFarming) // ����
            {
                anim.SetTrigger("doDigDown");
                isDigging = true;
                digDelay = 0f;
                curtivatePoint[0].gameObject.SetActive(true);
                effectSound[7].Play();
                StartCoroutine(ResetDig());
            }
            else if (toolIndex == 3) // ä��
            {
                // ���� ������ ������ ���� (OverlapSphere ���)

                Collider[] checkOre = Physics.OverlapSphere(transform.position, MiningOre.allowedDistance);

                foreach (Collider hitOre in checkOre)
                {
                    if (hitOre.CompareTag("Ore")) // 'Ore' �±װ� �ִ��� Ȯ�� (�ִٸ� ä�� ����)
                    {
                        anim.SetTrigger("doDigDown");
                        isDigging = true;
                        digDelay = 0f;
                        effectSound[8].Play();
                        StartCoroutine(ResetDig());
                    }
                }
            }
        }

        else if (toolIndex == 2 && dDown && isPickReady && !isHarvest && !player.pAttack.IsAttacking)
        {// ���� ��Ȯ
            player.rigid.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            anim.SetTrigger("doPick");
            isPicking = true;
            pickDelay = 0f;
            if (isPicking && !effectSound[11].isPlaying)
            {
                effectSound[11].Play();
            }
            else if ((!isPicking && effectSound[11].isPlaying))
            {
                effectSound[11].Stop();
            }
            
            StartCoroutine(ResetPick());
        }
        else if (toolIndex == 4 && canFarming && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// ���ı�
            anim.SetTrigger("doDigUp");
            isDigging = true;
            digDelay = 0f;
            curtivatePoint[1].gameObject.SetActive(true);
            
            if (isDigging && !effectSound[9].isPlaying)
            {
                effectSound[9].Play();
            }
            else if ((!isDigging && effectSound[9].isPlaying))
            {
                effectSound[9].Stop();
            }
            StartCoroutine(ResetDigUp());
        }
        else if(toolIndex == 5 && canFarming && dDown && isHarvestReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// �۹� ��Ȯ�ϱ�
            anim.SetTrigger("doHarvest");
            isHarvest = true;
            pointH.gameObject.SetActive(true);
            harvestDelay = 0f;
            
            if (isHarvest && !effectSound[10].isPlaying)
            {
                effectSound[10].Play();
            }
            else if ((!isHarvest && effectSound[10].isPlaying))
            {
                effectSound[10].Stop();
            }
            StartCoroutine(ResetHarvest());
        }
    }

    /// <summary>
    /// �κ��丮�κ��� ������ ���� ���� ����Ǿ����� ó���Ǵ� �̺�Ʈ ó����
    /// </summary>
    /// <param name="slotIdx"></param>
    private void OnItemSelectionChanged(int slotIdx)
    {
        // ���� ����ִ� ������ �����
        if (grabbedItem != null)
            Destroy(grabbedItem.gameObject);

        ItemIdEnum item = InventoryManager.getItemId(slotIdx);
        if (item == ItemIdEnum.TEST_NULL_ITEM)
        {
            // �ƹ��͵� �������� ���� ���¶��
            grabbedItem = null;
            isGrabbing = false;
            grabbingSlotIdx = -1;
            return;
        }

        // ���𰡸� �����ߴٸ� �տ� �����
        grabbedItem = ItemPrefabSO.getItemPrefab(new ItemBundle(item, 1, false)).gameObject;
        grabbedItem.SetActive(false);
        isGrabbing = true;
        grabbingSlotIdx = slotIdx;
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
        ItemBundle[] inventoryItems = InventoryManager.popAllItems();
        if (inventoryItems.Length > 0)
            ItemThrowManager.throwItem(new ItemBundle(new ItemPackage(inventoryItems), 1, false));
        anim.SetTrigger("doDie");
        isDie = true;
        player.pMove.IsWalking = false;
        player.pMove.IsRunning = false;
    }
    public void Swap()
    {
        if (!isEating && !isDigging && !isPicking && !isPickingUp && !isHarvest && !isPlant) {
            int currentIndex = toolIndex;
            if (sDown[1]) // �ָ�
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 0;
                isGrabbing = false;
            }
            if (sDown[2]) // ����
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 1;
                isGrabbing = false;
            }
            if (sDown[3]) // ����â(���� ��Ȯ��)
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 2;
                isGrabbing = false;
            }
            if (sDown[4]) // ���
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 3;
                isGrabbing = false;
            }
            if (sDown[5]) // ��
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 4;
                isGrabbing = false;
            }
            if (sDown[6]) // ��
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 5;
                isGrabbing = false;
            }
            if (sDown[7]) // ��
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 6;
                isGrabbing = false;
            }
            if (sDown[0]) // �̽��Ϳ���
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 7;
                isGrabbing = false;
            }

            // ��� �ִ� ������ ó��
            if (isGrabbing)
            {
                toolIndex = 0;

                tools[currentIndex].gameObject.SetActive(false);
                grabbedItem.transform.SetParent(ItemPoint, true);
                grabbedItem.transform.localPosition = Vector3.zero;
                grabbedItem.SetActive(true);
            }
            else
            {
                if (grabbedItem != null)
                    Destroy(grabbedItem.gameObject);
                grabbedItem = null;
            }

            if (!isGrabbing)
            {
                tools[toolIndex].gameObject.SetActive(true);
            }
        }
    }
    IEnumerator ResetDig()
    {
        if(toolIndex == 3)
            yield return new WaitForSeconds(1f);
        else
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
        yield return new WaitForSeconds(1f); 
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
            GetItem(pickedItem.GetComponent<ItemPrefabID>()); // �ֿ� �������� �κ��丮 ó��
            pickedItem.transform.SetParent(null);
            pickedItem.gameObject.SetActive(false);
            pickedItem = null; // Reset the reference to the item
        }
    }

    private void GetItem(ItemPrefabID itemPrefabId)
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
                WarningManager.startWarning();
            }
        }
    }

    IEnumerator ResetEat()
    {
        yield return new WaitForSeconds(1.5f);
        isEating = false;
        InventoryManager.takeItem(grabbingSlotIdx, 1);
    }

    IEnumerator ResetHarvest()
    {
        yield return new WaitForSeconds(0.7f);
        pointH.gameObject.SetActive(false);
        isHarvest = false;
    }

    IEnumerator ResetPlant()
    {
        yield return new WaitForSeconds(4f);
        isPlant = false;
        InventoryManager.takeItem(grabbingSlotIdx, 1);
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
    }

    IEnumerator Planting()
    {
        yield return new WaitForSeconds(1f);
        ItemPrefabID seed = ItemPoint.GetComponentInChildren<ItemPrefabID>();
        bool isSeed = seed.itemType == ItemIdEnum.CARROT_SEED || seed.itemType == ItemIdEnum.RICE_SEED || seed.itemType == ItemIdEnum.TOMATO_SEED ||
                      seed.itemType == ItemIdEnum.LEMMON_SEED || seed.itemType == ItemIdEnum.WATERMELON_SEED;
        // ���� ������ ������ ���� (OverlapSphere ���)
        Collider nearestCollider = null;
        float nearestDistance = Mathf.Infinity;
        Collider[] checkGround = Physics.OverlapSphere(transform.position, 1f); // �÷��̾� �ֺ� 1���� ����
        foreach (Collider collider in checkGround)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Harvest")) // 'Harvest' �±װ� �ִ��� Ȯ��
            {
                // �� �ݶ��̴����� �Ÿ� ���
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestCollider = collider;
                }
            }
        }
        crop = nearestCollider.GetComponent<Crops>();
        if (crop == null)
        {
            yield break;
        }
        yield return new WaitForSeconds(6f);
        // ���� ��� �ִ� �������� �������� Ȯ��
        if (isSeed)
        {
            switch(seed.itemType)
            {
                case ItemIdEnum.CARROT_SEED:
                    crop.CropIndex = 0;
                    break;
                case ItemIdEnum.RICE_SEED:
                    crop.CropIndex = 1;
                    break;
                case ItemIdEnum.TOMATO_SEED:
                    crop.CropIndex = 2;
                    break;
                case ItemIdEnum.LEMMON_SEED:
                    crop.CropIndex = 3;
                    break;
                case ItemIdEnum.WATERMELON_SEED:
                    crop.CropIndex = 4;
                    break;
            }

        }
    }
    // ���� ��Ȯ ���� + �ǰ� ����
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
                    effectSound[0].Play(); // 0�� ȿ���� ���
                    StartCoroutine(PlaySoundWithDelay(effectSound[3], 1.0f)); // 1�� �� 1�� ȿ���� ���
                }
            }
            else if (other.tag == "BossAttack")
            {
                anim.SetTrigger("doHit");
                player.health -= 6;
                effectSound[1].Play();
            }
            else if (other.tag == "BossRock")
            {
                anim.SetTrigger("doHit");
                player.health -= 10;
                effectSound[1].Play();
            }
            else if(other.tag == "EnemyAttack")
            {
                anim.SetTrigger("doHit");
                player.health -= 2;
                effectSound[1].Play();
            }
            if (player.health <= 0)
            {
                Die();
                effectSound[2].Play();
            }
        }
    }
    void OnCollisionStay(Collision collision)
    {
        // ���� �÷��̾ ��� �ִ� �ٴ��� ���������� Ȯ��
        if (collision.gameObject.layer == LayerMask.NameToLayer("Harvest"))
        {
            canFarming = true;
        }
        // ���� �÷��̾ ��� �ִ� �ٴ��� ���۵� ������ Ȯ��
        if (collision.gameObject.CompareTag("Plant"))
        {
            canPlant = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // ���� �÷��̾ ��� �ִ� �ٴ��� ���������� Ȯ��
        if (collision.gameObject.layer == LayerMask.NameToLayer("Harvest"))
        {
            canFarming = false;
        }
        // ���� �÷��̾ ��� �ִ� �ٴ��� ���۵� ������ Ȯ��
        if (collision.gameObject.CompareTag("Plant"))
        {
            canPlant = false;
        }
    }
    private IEnumerator PlaySoundWithDelay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���
        audioSource.Play(); // ������ ȿ���� ���
    }
    private IEnumerator StopSoundWithDelay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���
        audioSource.Stop(); // ������ ȿ���� ���
    }
}
